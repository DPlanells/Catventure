using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    // Instancia �nica (Singleton) del GameManager
    public static GameManager instance;

    // Bandera para verificar si la inicializaci�n ha sido completada
    private bool isInitialized = false;

    // Variables del estado del juego
    private int nVidas; // N�mero de vidas del jugador
    public TMP_Text croquetas; // Referencia al texto que muestra el n�mero de croquetas
    public Animator Order; // Animador para las croquetas
    private int nCroquetas; // Contador de croquetas recogidas

    // Referencias a objetos de la escena
    private GameObject player; // Referencia al jugador
    private ThirdPersonMovement scriptJugador; // Script de movimiento del jugador
    private GameObject UIHabilidad; // Referencia a la UI de habilidades
    public GameObject UIVidas; // Referencia al objeto de la UI de vidas
    private Animator UIAnimator; // Animador de la UI de habilidades
    private Animator UIVidasAnimator; // Animador de la UI de vidas
    private TMP_Text UIAText; // Texto de la UI de habilidades
    private TMP_Text UIVidasText1; // Texto principal del n�mero de vidas
    private TMP_Text UIVidasText2; // Texto secundario del n�mero de vidas
    private Animator UIVidasAnimator2; // Animador secundario de la UI de vidas
    private GameObject panelPausa; // Panel de pausa
    private Animator TextoPausa; // Animador del texto de pausa

    // Variables relacionadas con el sistema de guardado
    private SaveManager saveManager; // Administrador de guardados
    private GameLoader loader; // Cargador de datos guardados
    private int slot; // Slot de guardado seleccionado
    private Vector3 checkpointPosition; // �ltima posici�n de guardado
    private bool pausado; // Indica si el juego est� pausado

    // Flags para la inicializaci�n de la UI
    private bool uiInitialized = false; // Indica si la UI ha sido inicializada
    private bool needsUIUpdate = false; // Indica si es necesario actualizar la UI


    private void Awake()
    {
        // Configuraci�n del Singleton para asegurar una �nica instancia
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
            return;
        }

        // Registrar el evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Configuraci�n inicial de vidas y croquetas
        nVidas = 7;
        nCroquetas = 0;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reiniciar el estado al cambiar de escena
        isInitialized = false;

        // Si estamos en la escena principal del juego, inicializamos el estado
        if (scene.name == "MainScene")
        {
            StartCoroutine(InitializeGameState());
        }
        else
        {
            // Desactivar el panel de pausa y reiniciar el estado
            if (panelPausa != null) panelPausa.SetActive(false);
            pausado = false;
            Time.timeScale = 1; // Asegurar que el tiempo corre normalmente
        }
    }

    private IEnumerator InitializeGameState()
    {
        // Esperar un frame para asegurar que todos los objetos est�n listos
        yield return null;

        // Inicializar el sistema de guardado y cargado
        saveManager = FindObjectOfType<SaveManager>();
        loader = FindObjectOfType<GameLoader>();

        // Encontrar al jugador en la escena
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            scriptJugador = player.GetComponent<ThirdPersonMovement>();
        }

        // Inicializar los componentes de la UI
        if (InitializeUIComponents())
        {
            // Cargar datos guardados o iniciar un nuevo juego
            if (PlayerPrefs.HasKey("SlotSeleccionado"))
            {
                slot = PlayerPrefs.GetInt("SlotSeleccionado");
                if (saveManager != null && saveManager.SaveSlotExists(slot))
                {
                    LoadGameData(slot);
                }
                else
                {
                    StartNewGame();
                }
            }
            else
            {
                StartNewGame();
            }

            isInitialized = true; // Marcar la inicializaci�n como completada
        }
        else
        {
            Debug.LogError("Fallo al inicializar componentes UI");
        }
    }

    private bool InitializeUIComponents()
    {
        try
        {
            // Obtener referencias a los objetos de la UI en la escena
            panelPausa = GameObject.Find("Panel Pausa");
            TextoPausa = GameObject.FindGameObjectWithTag("TextoPausa")?.GetComponent<Animator>();
            croquetas = GameObject.FindGameObjectWithTag("Cont")?.GetComponent<TMP_Text>();
            Order = GameObject.FindGameObjectWithTag("Order")?.GetComponent<Animator>();
            UIHabilidad = GameObject.FindGameObjectWithTag("UIHabilidad");
            UIVidas = GameObject.FindGameObjectWithTag("UIVidas");
            UIVidasText1 = GameObject.FindGameObjectWithTag("UIVidasText1")?.GetComponent<TMP_Text>();
            UIVidasText2 = GameObject.FindGameObjectWithTag("UIVidasText2")?.GetComponent<TMP_Text>();

            // Inicializar animadores de la UI
            if (UIHabilidad != null)
            {
                UIAnimator = UIHabilidad.GetComponent<Animator>();
                UIAText = UIHabilidad.transform.GetChild(0).GetComponent<TMP_Text>();
            }

            if (UIVidas != null)
            {
                UIVidasAnimator = UIVidas.GetComponent<Animator>();
                UIVidasAnimator2 = GameObject.FindGameObjectWithTag("UIVidasText2")?.GetComponent<Animator>();
            }

            return ValidateUIComponents(); // Validar que todos los componentes necesarios fueron encontrados
        }
        catch (Exception e)
        {
            Debug.LogError($"Error inicializando UI: {e.Message}");
            return false;
        }
    }

    private bool ValidateUIComponents()
    {
        // Verificar que todos los componentes necesarios de la UI est�n presentes
        return croquetas != null && Order != null && UIHabilidad != null && UIVidas != null &&
               UIVidasText1 != null && UIVidasText2 != null && UIAnimator != null &&
               UIVidasAnimator != null && UIAText != null && UIVidasAnimator2 != null;
    }


    private void Update()
    {
        // Intentar inicializar la UI si a�n no lo ha sido pero se requiere
        if (needsUIUpdate && !uiInitialized)
        {
            InitializeUIComponents();
        }

        // Manejar la pausa del juego
        if (Input.GetButtonDown("Pause"))
        {
            if (!pausado) PauseGame();
            else ResumeGame();
        }
    }



    // M�todo para iniciar un nuevo juego con valores iniciales
    private void StartNewGame()
    {
        nVidas = 7;
        nCroquetas = 0;
        pausado = false;
        Time.timeScale = 1; // Asegurar que el tiempo corre normalmente

        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }

        UpdateUIValues(); // Actualizar los valores en la UI
        Debug.Log("Nueva partida iniciada");
    }

    // Carga los datos del juego desde un slot espec�fico
    private void LoadGameData(int slotToLoad)
    {
        try
        {
            // Intenta cargar los datos guardados desde el slot especificado
            SaveData data = loader.LoadProgress(slotToLoad);
            if (data != null)
            {
                // Asignar las variables del juego desde los datos cargados
                nVidas = data.lives;
                nCroquetas = data.coins;

                pausado = false; // Reinicia el estado de pausa
                Time.timeScale = 1; // Asegura que el tiempo est� corriendo

                if (scriptJugador != null)
                {
                    // Configura las habilidades del jugador seg�n los datos guardados
                    scriptJugador.setRun(data.canRun);
                    scriptJugador.setJump(data.canJump);
                    scriptJugador.setAttack(data.canAttack);
                    scriptJugador.setLaunch(data.canLaunch);
                    scriptJugador.setDoubleJump(data.canDoubleJump);

                    //Elimina del mapa los pescados de las habilidades ya encontradas
                    quitarPescados();
                }

                // Reubica al jugador al �ltimo checkpoint si corresponde
                if (data.checkpointPosition != Vector3.zero && player != null)
                {
                    player.transform.position = data.checkpointPosition;
                }

                if (panelPausa != null)
                {
                    panelPausa.SetActive(false); // Desactiva el men� de pausa
                }

                UpdateUIValues(); // Actualiza la interfaz con los nuevos valores
                Debug.Log($"Partida cargada desde slot {slotToLoad}");
            }
            else
            {
                // Si no hay datos guardados, comienza un nuevo juego
                StartNewGame();
            }
        }
        catch (Exception e)
        {
            // Manejo de errores en caso de fallo al cargar los datos
            Debug.LogError($"Error cargando partida: {e.Message}");
            StartNewGame(); // Comienza un nuevo juego en caso de error
        }
    }

    private void quitarPescados()
    {
        try
        {
            if(scriptJugador != null)
            {
                if (scriptJugador.canRun)
                {
                    GameObject pescado = GameObject.Find("PescadoCorrer");
                    Destroy(pescado);
                }
                if (scriptJugador.canJump)
                {
                    GameObject pescado = GameObject.Find("PescadoSaltar");
                    Destroy(pescado);
                }
                if (scriptJugador.canAttack)
                {
                    GameObject pescado = GameObject.Find("PescadoAtacar");
                    Destroy(pescado);
                }
                if (scriptJugador.canLaunch)
                {
                    GameObject pescado = GameObject.Find("PescadoLanzarse");
                    Destroy(pescado);
                }
                if (scriptJugador.canDoubleJump)
                {
                    GameObject pescado = GameObject.Find("PescadoSaltarDoble");
                    Destroy(pescado);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Ha aparecido el siguiente error al eliminar pescados de la escena: " + e.Message);
        }
    }


    // Actualiza los valores de la interfaz de usuario relacionados con vidas y croquetas
    private void UpdateUIValues()
    {
        if (croquetas != null) croquetas.text = nCroquetas.ToString();
        if (UIVidasText1 != null) UIVidasText1.text = nVidas.ToString();
        if (UIVidasText2 != null) UIVidasText2.text = nVidas.ToString();
    }

    // Limpia los eventos al destruir la instancia
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    // Pausa el juego y actualiza el estado correspondiente
    public void PauseGame()
    {
        if (!isInitialized || pausado) return; // Evita pausar si ya est� pausado o no inicializado

        Time.timeScale = 0; // Detiene el tiempo del juego
        Debug.Log("Juego pausado");
        pausado = true;

        if (panelPausa != null) panelPausa.SetActive(true); // Muestra el men� de pausa
        if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Baja");
        if (Order != null) Order.SetTrigger("Baja");
        if (TextoPausa != null) TextoPausa.SetTrigger("Pausado");
    }

    // Reanuda el juego despu�s de haber sido pausado
    public void ResumeGame()
    {
        if (!pausado) return; // Evita reanudar si no est� pausado

        Time.timeScale = 1; // Restaura el tiempo del juego
        Debug.Log("Juego reanudado");
        pausado = false;

        if (panelPausa != null) panelPausa.SetActive(false); // Oculta el men� de pausa
        if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Sube");
        if (Order != null) Order.SetTrigger("Sube");
        if (TextoPausa != null) TextoPausa.SetTrigger("Despausado");
    }

    // Incrementa el contador de croquetas y actualiza la interfaz
    public void sumarCroqueta()
    {
        nCroquetas += 1; // Incrementa la cantidad de croquetas
        Order.Play("OrderBaja");
        croquetas.text = nCroquetas.ToString(); // Actualiza el texto de croquetas en la interfaz
        Debug.Log("Croquetas recogidas = " + nCroquetas);
        Order.SetTrigger("Sube");
    }


    // A�ade una nueva habilidad al jugador
    public void AddAbility(AbilityType newAbility)
    {
        if (!isInitialized || scriptJugador == null) return;

        scriptJugador.AddAbility(newAbility); // Agrega la habilidad al jugador

        if (UIAText != null)
        {
            // Muestra un mensaje en la interfaz dependiendo de la habilidad adquirida
            switch (newAbility)
            {
                case AbilityType.Correr:
                    UIAText.text = "�Ahora Catti puede correr con LB!";
                    break;
                case AbilityType.Saltar:
                    UIAText.text = "�Ahora Catti puede saltar con la A!";
                    break;
                case AbilityType.Atacar:
                    UIAText.text = "�Ahora Catti puede atacar con LT!";
                    break;
                case AbilityType.Lanzarse:
                    UIAText.text = "�Ahora Catti puede lanzarse en el aire con X";
                    break;
                case AbilityType.DobleSalto:
                    UIAText.text = "�Ahora Catti puede hacer un salto en el aire!";
                    break;
            }
        }

        if (UIAnimator != null)
        {
            UIAnimator.Play("UIHabilidadSubir"); // Activa una animaci�n en la interfaz
        }
    }

    // Devuelve la cantidad de vidas actuales del jugador
    public int getVidas()
    {
        return nVidas;
    }

    // Devuelve la cantidad de croquetas actuales del jugador
    public int getCroquetas()
    {
        return nCroquetas;
    }

    // Reduce las vidas del jugador por un da�o recibido y actualiza la interfaz
    public void da�arJugador(int danyo)
    {
        if (!isInitialized || scriptJugador == null) return;

        if (!scriptJugador.getVulnerable()) // Verifica si el jugador no es vulnerable
        {
            nVidas -= danyo; // Resta las vidas seg�n el da�o recibido

            if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Baja");
            if (UIVidasText1 != null) UIVidasText1.text = nVidas.ToString();
            if (UIVidasText2 != null) UIVidasText2.text = (nVidas + 1).ToString();
            if (UIVidasAnimator2 != null) UIVidasAnimator2.SetTrigger("NumeroCae");

            scriptJugador.activarVulnerabilidad(); // Activa la vulnerabilidad del jugador temporalmente

            if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Sube");

            if (nVidas <= 0)
            {
                Morir(); // Llama al m�todo de muerte si no hay vidas restantes
            }
        }
    }

    // Reduce las vidas del jugador espec�ficamente por da�o de agua
    public void da�arJugadorAgua(int danyo)
    {
        nVidas -= danyo;

        if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Baja");
        if (UIVidasText1 != null) UIVidasText1.text = nVidas.ToString();
        if (UIVidasText2 != null) UIVidasText2.text = (nVidas + 1).ToString();
        if (UIVidasAnimator2 != null) UIVidasAnimator2.SetTrigger("NumeroCae");

        if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Sube");

        if (nVidas <= 0)
        {
            Morir();
        }
    }

    // Maneja la l�gica cuando el jugador pierde todas las vidas
    private void Morir()
    {
        Debug.Log("Partida finalizada, jugador muerto");
        EndGame(); // Llama al m�todo para finalizar la partida
    }

    // Finaliza el juego y regresa al men� principal
    public void EndGame()
    {
        Debug.Log("Volviendo al men� principal...");
        SceneManager.LoadScene("Sandbox menu"); // Cambia a la escena del men� principal
    }



    // Guarda el progreso del juego en un slot espec�fico
    public void SaveProgress(int slot)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Intentando guardar antes de la inicializaci�n completa");
            return;
        }

        if (scriptJugador == null)
        {
            Debug.LogError("No se puede guardar: scriptJugador es null");
            return;
        }

        SaveData data = new SaveData
        {
            lives = this.nVidas,
            coins = this.nCroquetas,
            canRun = scriptJugador.getRun(),
            canJump = scriptJugador.getJump(),
            canAttack = scriptJugador.getAttack(),
            canLaunch = scriptJugador.getLaunch(),
            canDoubleJump = scriptJugador.getDoubleJump(),
            checkpointPosition = checkpointPosition
        };

        saveManager.SaveGame(data, slot); // Guarda los datos en el slot especificado
    }

    // Establece el slot de guardado actual
    public void setSlotGuardado(int slot)
    {
        this.slot = slot;
    }

    // Establece la posici�n del �ltimo checkpoint alcanzado
    public void setCheckPoint(Vector3 position)
    {
        checkpointPosition = position;
    }

    // Devuelve el slot de guardado actual
    public int getSlotGuardado()
    {
        return this.slot;
    }

    public void volverMenuPrincipal()
    {
        SceneManager.LoadScene("SandBox Menu");
    }
}

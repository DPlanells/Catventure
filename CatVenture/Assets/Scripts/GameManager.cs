using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Singleton instance
    public static GameManager instance;

    private bool isInitialized = false;

    private int nVidas;
    public TMP_Text croquetas;
    public Animator Order;
    private int nCroquetas;



    private GameObject player;
    private ThirdPersonMovement scriptJugador;
    private GameObject UIHabilidad;
    public GameObject UIVidas;
    private Animator UIAnimator;
    private Animator UIVidasAnimator;
    private TMP_Text UIAText;
    private TMP_Text UIVidasText1;
    private TMP_Text UIVidasText2;
    private Animator UIVidasAnimator2;
    private GameObject panelPausa;
    private Animator TextoPausa;
    //Guardado
    private SaveManager saveManager;
    private GameLoader loader;
    private int slot;
    private Vector3 checkpointPosition;
    private Boolean pausado;

    private bool uiInitialized = false;
    private bool needsUIUpdate = false;


    private void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Registrar para eventos de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Inicialización básica del juego
        nVidas = 7;
        nCroquetas = 0;

        // Registramos un callback para cuando la escena termine de cargar
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset estado
        isInitialized = false;

        // Solo proceder si estamos en la escena del juego
        if (scene.name == "MainScene") // Ajusta al nombre de tu escena
        {
            StartCoroutine(InitializeGameState());
        }
        else
        {
            if (panelPausa != null)
            {
                panelPausa.SetActive(false); // Asegurar que el panel de pausa esté desactivado
            }
            pausado = false; // Reiniciar estado de pausa
            Time.timeScale = 1; // Asegurar que el tiempo esté corriendo
        }
    }

    private IEnumerator InitializeGameState()
    {
        // Esperar un frame para asegurar que todos los objetos estén en la escena
        yield return null;

        // Inicializar managers
        saveManager = FindObjectOfType<SaveManager>();
        loader = FindObjectOfType<GameLoader>();

        // Encontrar al jugador
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            scriptJugador = player.GetComponent<ThirdPersonMovement>();
        }

        // Inicializar UI
        if (InitializeUIComponents())
        {
            // Cargar datos o iniciar nueva partida
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

            isInitialized = true;
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
            panelPausa = GameObject.Find("Panel Pausa");
            TextoPausa = GameObject.FindGameObjectWithTag("TextoPausa")?.GetComponent<Animator>();

            croquetas = GameObject.FindGameObjectWithTag("Cont")?.GetComponent<TMP_Text>();
            Order = GameObject.FindGameObjectWithTag("Order")?.GetComponent<Animator>();
            UIHabilidad = GameObject.FindGameObjectWithTag("UIHabilidad");
            UIVidas = GameObject.FindGameObjectWithTag("UIVidas");
            UIVidasText1 = GameObject.FindGameObjectWithTag("UIVidasText1")?.GetComponent<TMP_Text>();
            UIVidasText2 = GameObject.FindGameObjectWithTag("UIVidasText2")?.GetComponent<TMP_Text>();

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

            return ValidateUIComponents();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error inicializando UI: {e.Message}");
            return false;
        }
    }

    private bool ValidateUIComponents()
    {
        return croquetas != null && Order != null && UIHabilidad != null && UIVidas != null &&
               UIVidasText1 != null && UIVidasText2 != null && UIAnimator != null &&
               UIVidasAnimator != null && UIAText != null && UIVidasAnimator2 != null;
    }


    // 2. Sistema de polling para asegurar la inicialización de UI
    private void Update()
    {
        // Si necesitamos actualizar la UI y aún no está inicializada, intentamos de nuevo
        if (needsUIUpdate && !uiInitialized)
        {
            InitializeUIComponents();
        }

        // El resto de la lógica del Update...
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausado) PauseGame();
            else ResumeGame();
        }
    }

    

    private void StartNewGame()
    {
        nVidas = 7;
        nCroquetas = 0;
        pausado = false;
        Time.timeScale = 1;

        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }

        UpdateUIValues();
        Debug.Log("Nueva partida iniciada");
    }

    private void LoadGameData(int slotToLoad)
    {
        try
        {
            SaveData data = loader.LoadProgress(slotToLoad);
            if (data != null)
            {
                nVidas = data.lives;
                nCroquetas = data.coins;

                pausado = false; // Reiniciar estado de pausa
                Time.timeScale = 1; // Asegurar que el tiempo esté corriendo

                if (scriptJugador != null)
                {
                    scriptJugador.setRun(data.canRun);
                    scriptJugador.setJump(data.canJump);
                    scriptJugador.setAttack(data.canAttack);
                    scriptJugador.setLaunch(data.canLaunch);
                    scriptJugador.setDoubleJump(data.canDoubleJump);
                }

                if (data.checkpointPosition != Vector3.zero && player != null)
                {
                    player.transform.position = data.checkpointPosition;
                }

                if (panelPausa != null)
                {
                    panelPausa.SetActive(false); // Desactivar menú de pausa
                }

                UpdateUIValues();
                Debug.Log($"Partida cargada desde slot {slotToLoad}");
            }
            else
            {
                StartNewGame();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error cargando partida: {e.Message}");
            StartNewGame();
        }
    }

    private void UpdateUIValues()
    {
        if (croquetas != null) croquetas.text = nCroquetas.ToString();
        if (UIVidasText1 != null) UIVidasText1.text = nVidas.ToString();
        if (UIVidasText2 != null) UIVidasText2.text = nVidas.ToString();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }








    // Método para pausar el juego
    public void PauseGame()
    {
        if (!isInitialized || pausado) return;

        Time.timeScale = 0;
        Debug.Log("Juego pausado");
        pausado = true;

        if (panelPausa != null) panelPausa.SetActive(true);
        if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Baja");
        if (Order != null) Order.SetTrigger("Baja");
        if (TextoPausa != null) TextoPausa.SetTrigger("Pausado");
    }

    // Método para reanudar el juego
    public void ResumeGame()
    {
        if (!pausado) return; // Evitar reanudar si no está pausado
        Time.timeScale = 1; // Reanudar el tiempo
        Debug.Log("Juego reanudado");
        pausado = false;
        if (panelPausa != null)
        {
            panelPausa.SetActive(false); // Cerrar menú de pausa
        }
        if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Sube");
        if (Order != null) Order.SetTrigger("Sube");
        if (TextoPausa != null) TextoPausa.SetTrigger("Despausado");
    }

    public void sumarCroqueta()
    {
        nCroquetas += 1;
        Order.Play("OrderBaja");
        croquetas.text = nCroquetas.ToString();
        Debug.Log("Croquetas recogidas = " + nCroquetas);
        Order.SetTrigger("Sube");
    }

    //Añade una habilidad nueva al jugador
    public void AddAbility(AbilityType newAbility)
    {
        if (!isInitialized || scriptJugador == null) return;

        scriptJugador.AddAbility(newAbility);

        if (UIAText != null)
        {
            switch (newAbility)
            {
                case AbilityType.Correr:
                    UIAText.text = "Ahora Catti puede correr con Shift!";
                    break;
                case AbilityType.Saltar:
                    UIAText.text = "Ahora Catti puede saltar con la barra espaciadora!";
                    break;
                case AbilityType.Atacar:
                    UIAText.text = "Ahora Catti puede atacar con J!";
                    break;
            }
        }

        if (UIAnimator != null)
        {
            UIAnimator.Play("UIHabilidadSubir");
        }
    }

    public int getVidas()
    {
        return nVidas;
    }

    public int getCroquetas()
    {
        return nCroquetas;
    }

    public void dañarJugador(int danyo)
    {
        if (!isInitialized || scriptJugador == null) return;

        if (scriptJugador.getVulnerable() == false)
        {
            nVidas -= danyo;

            if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Baja");
            if (UIVidasText1 != null) UIVidasText1.text = nVidas.ToString();
            if (UIVidasText2 != null) UIVidasText2.text = (nVidas + 1).ToString();
            if (UIVidasAnimator2 != null) UIVidasAnimator2.SetTrigger("NumeroCae");

            scriptJugador.activarVulnerabilidad();

            if (UIVidasAnimator != null) UIVidasAnimator.SetTrigger("Sube");

            if (nVidas <= 0)
            {
                Morir();
            }
        }
    }

    public void dañarJugadorAgua(int danyo)
    {
        nVidas -= danyo;
        if (nVidas <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("Partida finalizada, jugador muerto");


        // Llamar al método que maneja el fin de la partida
        EndGame();
    }

    public void EndGame()
    {
        Debug.Log("Volviendo al menú principal...");

        // Transición a la escena del menú principal
        SceneManager.LoadScene("Sandbox menu");
    }


    public void SaveProgress(int slot)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Intentando guardar antes de la inicialización completa");
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

        saveManager.SaveGame(data, slot);
    }

    public void setSlotGuardado(int slot)
    {
        this.slot = slot;
    }

    public void setCheckPoint(Vector3 position)
    {
        checkpointPosition = position;
    }

    public int getSlotGuardado()
    {
        return this.slot;
    }

 

}

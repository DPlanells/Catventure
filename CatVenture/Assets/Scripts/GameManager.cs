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
    public GameObject panelPausa;
    public Animator TextoPausa;
    //Guardado
    private SaveManager saveManager;
    private GameLoader loader;
    private int slot;
    private Vector3 checkpointPosition;
    private Boolean pausado;
    private void Awake()
    {
        // Implementación del patrón Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject);  // Si ya hay una instancia, destruir la nueva
        }

        slot = 1;

        // Intentar encontrar al jugador automáticamente si no está asignado
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player' en la escena.");
            }

            
        }

        if (player != null)
        {
            scriptJugador = player.GetComponent<ThirdPersonMovement>();
            if (scriptJugador == null)
            {
                Debug.LogError("El objeto del jugador no tiene un componente ThirdPersonMovement.");
            }
        }

        saveManager = FindObjectOfType<SaveManager>();
        if (saveManager == null)
        {
            Debug.LogError("SaveManager no encontrado en la escena.");
            return;
        }

        loader = FindObjectOfType<GameLoader>();
        if (loader == null)
        {
            Debug.LogError("GameLoader no encontrado en la escena.");
            return;
        }

        // Recuperar el slot seleccionado
        if (PlayerPrefs.HasKey("SlotSeleccionado"))
        {
            slot = PlayerPrefs.GetInt("SlotSeleccionado");
            Debug.Log("Intentando cargar una partida");
            cargarPartida(slot); // Carga la partida automáticamente
        }
        else
        {
            Debug.LogWarning("No se encontró un slot seleccionado.");
        }
        loader = FindObjectOfType<GameLoader>();




        pausado = false;
        Time.timeScale = 1;  // Asegurarse de que el tiempo esté en marcha

        if (panelPausa != null)
        {
            panelPausa.SetActive(false); // Desactiva el panel desde el inicio
        }

        StartGame();
    }

    // Método para iniciar el juego
    public void StartGame()
    {
        // Lógica para iniciar el juego
        nVidas = 7;
        nCroquetas = 0;
        croquetas = GameObject.FindGameObjectWithTag("Cont").GetComponent<TMP_Text>();
        Order = GameObject.FindGameObjectWithTag("Order").GetComponent<Animator>();
        UIHabilidad = GameObject.FindGameObjectWithTag("UIHabilidad");
        UIAnimator= UIHabilidad.GetComponent<Animator>();
        UIAText = UIHabilidad.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        UIVidas = GameObject.FindGameObjectWithTag("UIVidas");
        UIVidasAnimator = UIVidas.GetComponent<Animator>();
        UIVidasText1 = GameObject.FindGameObjectWithTag("UIVidasText1").GetComponent<TMP_Text>();
        UIVidasText2 = GameObject.FindGameObjectWithTag("UIVidasText2").GetComponent<TMP_Text>();
        UIVidasAnimator2 = GameObject.FindGameObjectWithTag("UIVidasText2").GetComponent<Animator>();
        Debug.Log("Juego iniciado");
    }

    void Update() {

        if (Input.GetButtonDown("Pause")) { 
            if (!pausado)
                PauseGame();
            else ResumeGame();
        }
    }
    public void retomarJuego()
    {
        croquetas = GameObject.FindGameObjectWithTag("Cont").GetComponent<TMP_Text>();
        Order = GameObject.FindGameObjectWithTag("Order").GetComponent<Animator>();
        UIHabilidad = GameObject.FindGameObjectWithTag("UIHabilidad");
        UIAnimator = UIHabilidad.GetComponent<Animator>();
        UIAText = UIHabilidad.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        UIVidas = GameObject.FindGameObjectWithTag("UIVidas");
        UIVidasAnimator = UIVidas.GetComponent<Animator>();
        UIVidasText1 = GameObject.FindGameObjectWithTag("UIVidasText1").GetComponent<TMP_Text>();
        UIVidasText2 = GameObject.FindGameObjectWithTag("UIVidasText2").GetComponent<TMP_Text>();
        UIVidasAnimator2 = GameObject.FindGameObjectWithTag("UIVidasText2").GetComponent<Animator>();
        Debug.Log("Juego iniciado a partir del saveFile " + slot);
    }

    // Método para pausar el juego
    public void PauseGame()
    {
        if (pausado) return; // Evitar múltiples pausas

        // Lógica para pausar el juego
        Time.timeScale = 0;  // Pausar el tiempo
        Debug.Log("Juego pausado");
        pausado = true;
        panelPausa.SetActive(true);
        UIVidasAnimator.SetTrigger("Baja");
        Order.SetTrigger("Baja");
        TextoPausa.SetTrigger("Pausado");
    }

    // Método para reanudar el juego
    public void ResumeGame()
    {
        if (!pausado) return; // Evitar reanudar si no está pausado

        // Lógica para reanudar el juego
        Time.timeScale = 1;  // Reanudar el tiempo
        Debug.Log("Juego reanudado");
        pausado = false;
        UIVidasAnimator.SetTrigger("Sube");
        Order.SetTrigger("Sube");
        TextoPausa.SetTrigger("Despausado");
        panelPausa.SetActive(false);
    }

    public void sumarCroqueta()
    {
        nCroquetas += 1;
        Order.Play("OrderBaja");
        croquetas.text =nCroquetas.ToString();
        Debug.Log("Croquetas recogidas = " + nCroquetas);
        Order.SetTrigger("Sube");
    }

    //Añade una habilidad nueva al jugador
    public void AddAbility(AbilityType newAbility)
    {
        scriptJugador.AddAbility(newAbility);

        switch (newAbility) {

            case AbilityType.Correr:
                UIAText.text = "Ahora Catti puede correr con Shift!";
                Debug.Log("Ahora Catti puede correr con Shift!");
            break;

            case AbilityType.Saltar:
                UIAText.text = "Ahora Catti puede saltar con la barra espaciadora!";
            break;

            case AbilityType.Atacar:
                UIAText.text = "Ahora Catti puede atacar con J!";

            break;

            default: break;
        }

        UIAnimator.Play("UIHabilidadSubir");

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

        if (scriptJugador.getVulnerable() == false)
        {
            nVidas -= danyo;
            UIVidasAnimator.SetTrigger("Baja");
            UIVidasText1.text = nVidas.ToString();
            UIVidasText2.text = (nVidas+1).ToString();
            UIVidasAnimator2.SetTrigger("NumeroCae");
            scriptJugador.activarVulnerabilidad();
            UIVidasAnimator.SetTrigger("Sube");

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

    public void cargarPartida(int slot)
    {
        if (saveManager == null || loader == null)
        {
            Debug.LogError("SaveManager o GameLoader no encontrados en la escena. No se puede cargar la partida.");
            return;
        }

        if (!saveManager.SaveSlotExists(slot))
        {
            Debug.LogWarning($"No existe un archivo de guardado en el slot {slot}. Iniciando una nueva partida.");
            StartGame();
            return;
        }

        try
        {
            SaveData datosPartida = loader.LoadProgress(slot);

            if (datosPartida == null)
            {
                Debug.LogWarning($"El archivo de guardado en el slot {slot} está vacío o corrupto. Iniciando una nueva partida.");
                StartGame();
                return;
            }

            nVidas = datosPartida.lives;
            nCroquetas = datosPartida.coins;

            scriptJugador.setRun(datosPartida.canRun);
            scriptJugador.setJump(datosPartida.canJump);
            scriptJugador.setAttack(datosPartida.canAttack);
            scriptJugador.setLaunch(datosPartida.canLaunch);
            scriptJugador.setDoubleJump(datosPartida.canDoubleJump);

            checkpointPosition = datosPartida.checkpointPosition;

            if (checkpointPosition != Vector3.zero)
            {
                player.transform.position = checkpointPosition;
            }

            Debug.Log($"Partida cargada desde el slot {slot}.");
            retomarJuego();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al cargar la partida: {e.Message}");
        }

        Time.timeScale = 1;
        pausado = false;
    }


}

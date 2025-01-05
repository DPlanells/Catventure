using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    private GameObject UIVidas;
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
        loader = FindObjectOfType<GameLoader>();

        //cargarPartida(slot);
        pausado = false;
        panelPausa.SetActive(pausado);
    
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

        if (Input.GetKeyDown(KeyCode.Escape)) { 
            if(!pausado)
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
        // Lógica para reanudar el juego
        Time.timeScale = 1;  // Reanudar el tiempo
        Debug.Log("Juego reanudado");
        pausado = false;
        UIVidasAnimator.SetTrigger("Sube");
        Order.SetTrigger("Sube");
        TextoPausa.SetTrigger("Despausado");
        panelPausa.SetActive(false);
    }

    // Método para finalizar el juego
    public void EndGame()
    {
        Debug.Log("Juego terminado");

        //TODO Logica para volver al menú principal
    }

    public void sumarCroqueta()
    {
        nCroquetas += 1;
        Order.Play("OrderBaja");
        croquetas.text =nCroquetas.ToString();
        Debug.Log("Croquetas recogidas = " + nCroquetas);
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

        //TODO Menu para volver al menu principal, o reiniciar desde el último punto de guardado
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

    public void cargarPartida(int slot)
    {
        SaveData datosPartida = loader.LoadProgress(slot);

        if (datosPartida != null)
        {
            //Cargar los datos del save al manager
            nVidas = datosPartida.lives;
            nCroquetas = datosPartida.coins;

            
            scriptJugador.setRun(datosPartida.canRun);
            scriptJugador.setJump(datosPartida.canJump);
            scriptJugador.setAttack(datosPartida.canAttack);
            scriptJugador.setLaunch(datosPartida.canLaunch);


            scriptJugador.setPosicion(datosPartida.checkpointPosition);

            retomarJuego();
        }
        else
        {
            StartGame();
        }
    }

}

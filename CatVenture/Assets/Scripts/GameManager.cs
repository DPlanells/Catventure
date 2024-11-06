using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Singleton instance
    public static GameManager instance;

    private int nVidas;

    private int nCroquetas;

    private int nPescados;

    private GameObject player;
    private ThirdPersonMovement scriptJugador;


    private void Awake()
    {
        // Implementaci�n del patr�n Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject);  // Si ya hay una instancia, destruir la nueva
        }

        // Intentar encontrar al jugador autom�ticamente si no est� asignado
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("No se encontr� un objeto con la etiqueta 'Player' en la escena.");
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


        StartGame();
    }

    // M�todo para iniciar el juego
    public void StartGame()
    {
        // L�gica para iniciar el juego
        nVidas = 7;
        nCroquetas = 0;
        nPescados = 0;


        Debug.Log("Juego iniciado");
    }

    // M�todo para pausar el juego
    public void PauseGame()
    {
        // L�gica para pausar el juego
        Time.timeScale = 0;  // Pausar el tiempo
        Debug.Log("Juego pausado");
    }

    // M�todo para reanudar el juego
    public void ResumeGame()
    {
        // L�gica para reanudar el juego
        Time.timeScale = 1;  // Reanudar el tiempo
        Debug.Log("Juego reanudado");
    }

    // M�todo para finalizar el juego
    public void EndGame()
    {
        // L�gica para finalizar el juego
        Debug.Log("Juego terminado");
    }

    public void sumarCroqueta()
    {
        nCroquetas += 1;
    }

    //A�ade una habilidad nueva al jugador
    public void AddAbility(AbilityType newAbility)
    {
        scriptJugador.AddAbility(newAbility);
    }

    public int getVidas()
    {
        return nVidas;
    }

    public int getCroquetas()
    {
        return nCroquetas;
    }

    

}

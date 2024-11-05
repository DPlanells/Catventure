using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Singleton instance
    public static GameManager instance;

    private int nVidas { get; set; }

    private int nCroquetas { get; set; }

    private int nPescados { get; set; }



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
    }

    // Método para iniciar el juego
    public void StartGame()
    {
        // Lógica para iniciar el juego
        nVidas = 7;
        nCroquetas = 0;
        nPescados = 0;


        Debug.Log("Juego iniciado");
    }

    // Método para pausar el juego
    public void PauseGame()
    {
        // Lógica para pausar el juego
        Time.timeScale = 0;  // Pausar el tiempo
        Debug.Log("Juego pausado");
    }

    // Método para reanudar el juego
    public void ResumeGame()
    {
        // Lógica para reanudar el juego
        Time.timeScale = 1;  // Reanudar el tiempo
        Debug.Log("Juego reanudado");
    }

    // Método para finalizar el juego
    public void EndGame()
    {
        // Lógica para finalizar el juego
        Debug.Log("Juego terminado");
    }

    public void sumarCroqueta()
    {
        nCroquetas += 1;
    }

}

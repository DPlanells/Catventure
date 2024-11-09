using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyScript : MonoBehaviour
{
    public int danyoJugador = 1;
    public int vidas = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador (etiqueta "Player")
        if (other.CompareTag("Player"))
        {
            // Llama al GameManager para que actualice el contador de vidas del jugador
            GameManager.instance.dañarJugador(danyoJugador);

            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CroquetaScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador (etiqueta "Player")
        if (other.CompareTag("Player"))
        {
            // Llama al GameManager para que actualice el contador de croquetas
            GameManager.instance.sumarCroqueta();

            // Reproducir el sonido de recolección (si se ha asignado)
            

            // Destruir el objeto coleccionable
            Destroy(gameObject);
        }
    }

}

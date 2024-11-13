using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CroquetaScript : MonoBehaviour
{

    void Update()
    {
        transform.localRotation = Quaternion.Euler(Time.time * 100f, Time.time * 100f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador (etiqueta "Player")
        if (other.CompareTag("Player"))
        {
            // Llama al GameManager para que actualice el contador de croquetas
            GameManager.instance.sumarCroqueta();

            // Reproducir el sonido de recolecci�n (si se ha asignado)
            

            // Destruir el objeto coleccionable
            Destroy(gameObject);
        }
    }

}

using UnityEngine;

public class ScriptAgua : MonoBehaviour
{
    private float collisionTimer = 0f; // Temporizador para rastrear la duraci�n de la colisi�n.
    private bool isPlayerInWater = false; // Indica si el jugador est� dentro del trigger.

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInWater = true; // El jugador entra en el agua.
            Debug.Log("Se ha entrado en el agua");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPlayerInWater)
        {
            // Incrementar el temporizador basado en el tiempo transcurrido.
            collisionTimer += Time.deltaTime;

            // Reducir una vida cada 0.2 segundos mientras el jugador est� en el agua.
            if (collisionTimer >= 0.2f)
            {
                GameManager.instance.da�arJugador(1); // Llama al m�todo de da�o en el GameManager.

                // Reiniciar el temporizador.
                collisionTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInWater = false; // El jugador sale del agua.
            collisionTimer = 0f; // Reinicia el temporizador al salir del trigger.
            Debug.Log("Se ha salido del agua");
        }
    }
}


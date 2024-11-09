using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public int damage = 1;   // Da�o que causa al jugador
    public float timerDuration = 10f;

    private Vector3 direction;

    // Establece la direcci�n del proyectil al instanciarlo
    public void SetDirection(Vector3 targetDirection)
    {
        direction = targetDirection.normalized;
    }

    void Update()
    {
        // Mueve el proyectil en la direcci�n establecida
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si colisiona con el jugador
        if (other.CompareTag("Player"))
        {
            // Llamar al script de salud del jugador (debes tener un script llamado PlayerHealth en el jugador)
            GameManager.instance.da�arJugador(damage);

            // Destruye el proyectil despu�s de impactar
            Destroy(gameObject);
        }

        // Destruye el proyectil si colisiona con cualquier otra cosa
        if (other.CompareTag("Ground")) // Cambia "Obstacle" al tag adecuado si necesitas colisiones con otros objetos
        {
            Destroy(gameObject);
        }
    }

    public void activarTemporizador()
    {
        
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        
        yield return new WaitForSeconds(timerDuration); // Esperar el tiempo de invulnerabilidad

        Destroy(gameObject);

    }
}

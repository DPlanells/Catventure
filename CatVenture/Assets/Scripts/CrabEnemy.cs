using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad de movimiento
    public float moveDistance = 5.0f; // Distancia de movimiento a cada lado

    

    private Vector2 startPos;
    private int direction = 1;

    void Start()
    {
        // Guarda la posici�n inicial del cangrejo
        startPos = transform.position;
    
    }

    void Update()
    {
        Move();
        
    }

    private void Move()
    {
        // Mueve el cangrejo a la izquierda o derecha
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Cambia de direcci�n al alcanzar los l�mites
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1; // Invierte la direcci�n
            Flip();
        }
    }

    private void Flip()
    {
        // Invierte la escala en X para que el cangrejo mire en la direcci�n correcta
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    private void OnCollisionEnter(Collision other)
    {
        // Si el enemigo colisiona con el jugador, inflige da�o al jugador
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.da�arJugador(1);
        }
    }

}

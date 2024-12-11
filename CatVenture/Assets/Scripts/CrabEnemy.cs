using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad de movimiento
    public float moveDistance = 5.0f; // Radio m�ximo de movimiento desde el punto de aparici�n
    public float chaseDistance = 8.0f; // Distancia de persecuci�n del jugador
    public float stopChaseDistance = 10.0f; // Distancia para detener la persecuci�n
    private Transform player; // Referencia al jugador
    public float directionChangeInterval = 2.0f; // Tiempo entre cambios de direcci�n en segundos

    private Vector3 startPos;
    private Vector3 wanderDirection; // Direcci�n actual de deambular
    private float directionChangeTimer; // Temporizador para cambiar la direcci�n
    private State currentState;

    private enum State
    {
        Wander, // Deambular
        Chase,  // Perseguir al jugador
        Return  // Regresar al punto de aparici�n
    }

    void Start()
    {
        // Busca y asigna al jugador autom�ticamente
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontr� un objeto con el tag 'Player' en la escena.");
        }

        startPos = transform.position;
        currentState = State.Wander; // Estado inicial
        SetRandomDirection(); // Inicializa una direcci�n aleatoria
    }

    void Update()
    {
        // Cambia de estado seg�n la distancia al jugador
        float playerDistance = Vector3.Distance(player.position, startPos);

        switch (currentState)
        {
            case State.Wander:
                Wander();
                if (playerDistance <= chaseDistance)
                {
                    currentState = State.Chase;
                    speed = 3.0f;
                }
                break;

            case State.Chase:
                Chase();
                if (playerDistance > stopChaseDistance)
                {
                    currentState = State.Return;
                    speed = 2.0f;
                }
                break;

            case State.Return:
                ReturnToStart();
                if (Vector3.Distance(transform.position, startPos) < 0.1f)
                {
                    currentState = State.Wander;
                }
                break;
        }
    }

    private void Wander()
    {
        // Mueve al cangrejo en la direcci�n actual
        transform.Translate(wanderDirection * speed * Time.deltaTime, Space.World);

        // Cambia la direcci�n aleatoriamente cada cierto intervalo
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= directionChangeInterval)
        {
            SetRandomDirection();
            directionChangeTimer = 0f;
        }

        // Limita el movimiento dentro del rango permitido
        if (Vector3.Distance(transform.position, startPos) > moveDistance)
        {
            Vector3 directionToCenter = (startPos - transform.position).normalized;
            wanderDirection = new Vector3(directionToCenter.x, 0, directionToCenter.z); // Corrige la direcci�n hacia el centro
        }
    }

    private void SetRandomDirection()
    {
        // Genera una direcci�n aleatoria en los ejes X y Z
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // Direcci�n en un plano 2D
        wanderDirection = new Vector3(randomDirection.x, 0, randomDirection.y); // Convierte a 3D con eje Y fijo
    }

    private void Chase()
    {
        // Sigue al jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.Translate(new Vector3(directionToPlayer.x, 0, directionToPlayer.z) * speed * Time.deltaTime, Space.World);
    }

    private void ReturnToStart()
    {
        // Regresa al punto de aparici�n
        Vector3 directionToStart = (startPos - transform.position).normalized;
        transform.Translate(new Vector3(directionToStart.x, 0, directionToStart.z) * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Se ha colisionado con un cangrejo");

        // Si el enemigo colisiona con el jugador, inflige da�o al jugador
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.da�arJugador(1);
            Debug.Log("Se ha da�ado al jugador");
        }
    }
}
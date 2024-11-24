using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlyingEnemy : MonoBehaviour
{
    public float circleRadius = 5.0f; // Radio de las vueltas alrededor del punto inicial
    public float circleSpeed = 2.0f; // Velocidad de giro
    public float attackSpeed = 5.0f; // Velocidad durante la parábola de ataque
    public float returnSpeed = 3.0f; // Velocidad de retorno al punto inicial
    public float playerDetectionRange = 8.0f; // Rango para detectar al jugador
    public LayerMask obstacleMask; // Máscara para detectar obstáculos

    private Vector3 startPos; // Posición inicial
    private Transform player; // Referencia al jugador
    private float angle; // Ángulo actual para el movimiento circular
    private State currentState;

    private enum State
    {
        Circle,  // Volar en círculos alrededor del punto inicial
        Attack,  // Bajar para atacar al jugador en una parábola
        Return   // Subir de nuevo al punto inicial
    }

    void Start()
    {
        // Almacenar la posición inicial
        startPos = transform.position;

        // Buscar al jugador automáticamente
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'Player' en la escena.");
        }

        currentState = State.Circle; // Estado inicial
    }

    void Update()
    {
        // Si no hay jugador asignado, no hacer nada
        if (player == null) return;

        // Cambiar comportamiento basado en el estado actual
        switch (currentState)
        {
            case State.Circle:
                CircleAround();
                if (IsPlayerInRange())
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                PerformParabolicAttack();
                break;

            case State.Return:
                ReturnToStart();
                break;
        }
    }

    private void CircleAround()
    {
        // Movimiento circular alrededor del punto inicial
        angle += circleSpeed * Time.deltaTime;
        float x = startPos.x + Mathf.Cos(angle) * circleRadius;
        float z = startPos.z + Mathf.Sin(angle) * circleRadius;
        transform.position = new Vector3(x, startPos.y, z);
    }

    private bool IsPlayerInRange()
    {
        // Detecta si el jugador está dentro del rango horizontal (sin importar la coordenada Y)
        Vector3 playerHorizontalPosition = new Vector3(player.position.x, startPos.y, player.position.z);
        return Vector3.Distance(startPos, playerHorizontalPosition) <= playerDetectionRange;
    }

    private void PerformParabolicAttack()
    {
        // Calcular la dirección hacia el jugador en el plano XZ
        Vector3 targetXZ = new Vector3(player.position.x, startPos.y, player.position.z);
        Vector3 directionToPlayer = (targetXZ - transform.position).normalized;

        // Avanzar en la parábola hacia el jugador
        Vector3 parabolaDirection = new Vector3(directionToPlayer.x, -1, directionToPlayer.z).normalized;
        transform.Translate(parabolaDirection * attackSpeed * Time.deltaTime, Space.World);

        // Comprobar colisión con obstáculos
        if (Physics.Raycast(transform.position, parabolaDirection, out RaycastHit hit, attackSpeed * Time.deltaTime, obstacleMask))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                // Si se detecta un obstáculo que no sea el jugador, interrumpe el ataque
                currentState = State.Return;
                return;
            }
        }

        // Si el enemigo está suficientemente cerca del jugador, infligir daño y volver
        if (Vector3.Distance(transform.position, player.position) < 1.0f)
        {
            GameManager.instance.dañarJugador(1);
            currentState = State.Return;
        }
    }

    private void ReturnToStart()
    {
        // Subir directamente al punto inicial
        Vector3 directionToStart = (startPos - transform.position).normalized;
        transform.Translate(directionToStart * returnSpeed * Time.deltaTime, Space.World);

        // Si ha vuelto al punto inicial, reanudar el movimiento circular
        if (Vector3.Distance(transform.position, startPos) < 0.1f)
        {
            transform.position = startPos; // Asegurar alineación perfecta
            currentState = State.Circle;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Se ha colisionado con un enemigo volador");

        // Si el enemigo colisiona con el jugador, inflige daño
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.dañarJugador(1);
            currentState = State.Return;
            Debug.Log("Se ha dañado al jugador");
        }
    }
}
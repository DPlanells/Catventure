using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbejaScript : MonoBehaviour
{
    public float circleRadius = 0.5f; // Radio de las vueltas alrededor del punto inicial
    public float circleSpeed = 0.5f; // Velocidad de giro
    public float attackSpeed = 5.0f; // Velocidad durante la par�bola de ataque
    public float returnSpeed = 3.0f; // Velocidad de retorno al punto inicial
    public float playerDetectionRange = 5.0f; // Rango para detectar al jugador
    public LayerMask obstacleMask; // M�scara para detectar obst�culos

    private Vector3 startPos; // Posici�n inicial
    private Transform player; // Referencia al jugador
    private float angle; // �ngulo actual para el movimiento circular
    private State currentState;


    public AudioClip idleClip; // Sonido para el estado Idle
    public AudioClip attackClip; // Sonido para el estado Attack
    private AudioSource audioSource;

    private enum State
    {
        Idle,  // Volar en c�rculos alrededor del punto inicial
        Attack,  // Bajar para atacar al jugador en una par�bola
        Return   // Subir de nuevo al punto inicial
    }

    void Start()
    {
        // Almacenar la posici�n inicial
        startPos = transform.position;

        // Buscar al jugador autom�ticamente
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontr� un objeto con el tag 'Player' en la escena.");
        }

        currentState = State.Idle; // Estado inicial

        audioSource = GetComponent<AudioSource>();

        audioSource.clip = idleClip; // Clip inicial
        audioSource.loop = true; // Reproducci�n en bucle
        audioSource.Play();
    }

    void Update()
    {
        // Si no hay jugador asignado, no hacer nada
        if (player == null) return;

        // Cambiar comportamiento basado en el estado actual
        switch (currentState)
        {
            case State.Idle:
                CircleAround();
                if (IsPlayerInRange())
                {
                    currentState = State.Attack;
                    ChangeAudioClip(attackClip); // Cambiar al sonido de ataque
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
        // Detecta si el jugador est� dentro del rango horizontal (sin importar la coordenada Y)
        Vector3 playerHorizontalPosition = new Vector3(player.position.x, startPos.y, player.position.z);
        return Vector3.Distance(startPos, playerHorizontalPosition) <= playerDetectionRange;
    }

    private void PerformParabolicAttack()
    {
        // Calcular la direcci�n hacia el jugador en el plano XZ
        Vector3 targetXZ = new Vector3(player.position.x, startPos.y, player.position.z);
        Vector3 directionToPlayer = (targetXZ - transform.position).normalized;

        // Avanzar en la par�bola hacia el jugador
        Vector3 parabolaDirection = new Vector3(directionToPlayer.x, -1, directionToPlayer.z).normalized;
        transform.Translate(parabolaDirection * attackSpeed * Time.deltaTime, Space.World);

        // Comprobar colisi�n con obst�culos
        if (Physics.Raycast(transform.position, parabolaDirection, out RaycastHit hit, attackSpeed * Time.deltaTime, obstacleMask))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                // Si se detecta un obst�culo que no sea el jugador, interrumpe el ataque
                currentState = State.Return;
                return;
            }
        }

        // Si el enemigo est� suficientemente cerca del jugador, infligir da�o y volver
        if (Vector3.Distance(transform.position, player.position) < 1.0f)
        {
            GameManager.instance.da�arJugador(1);
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
            transform.position = startPos; // Asegurar alineaci�n perfecta
            currentState = State.Idle;

            ChangeAudioClip(idleClip); // Volver al sonido de vuelo
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Se ha colisionado con un enemigo volador");

        // Si el enemigo colisiona con el jugador, inflige da�o
        if (other.gameObject.CompareTag("Player"))
        {
            
            GameManager.instance.da�arJugador(1);
            currentState = State.Return;
            Debug.Log("Se ha da�ado al jugador");

            Destroy(gameObject);
        }
    }

    private void ChangeAudioClip(AudioClip newClip)
    {
        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}

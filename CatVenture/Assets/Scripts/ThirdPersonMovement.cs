using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    // Movimiento
    public CharacterController controller;
    public Transform cam;

    public static float walkSpeed = 6f;  // Velocidad normal
    private static float specialRunMultiplier = 2f;  // Multiplicador de velocidad especial para habilidad Correr
    public float runSpeed = walkSpeed * specialRunMultiplier;  // Velocidad al correr
    private float currentSpeed;

    private Vector3 direction;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Salto
    public float jumpHeight = 2f;  // Altura del salto
    public float gravity = -22f;  // Gravedad aplicada
    private Vector3 velocity;  // Velocidad para el movimiento vertical
    private Vector3 horizontalVelocity;  // Velocidad horizontal en el salto

    public Transform groundCheck;  // Punto para verificar si está en el suelo
    public float groundDistance = 0.4f;  // Distancia del punto de verificación del suelo
    public LayerMask groundMask;  // Capa que representa el suelo

    bool isGrounded;

    // Animación
    public Animator anim;

    // Habilidades
    //public enum AbilityType { Correr, Saltar, Atacar }
    private bool canRun = false;
    private bool canPerformJump = false;
    private bool canAttack = false;

    void Update()
    {

        ManejarMovimiento();
        ManejarAtaque();

        // Animación
        UpdateAnimationParameters(direction, currentSpeed);
    }

    private void ManejarMovimiento()
    {
        // Comprobar si está en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reiniciar la velocidad en Y cuando toca el suelo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Asegura que no siga cayendo infinitamente
            horizontalVelocity = Vector3.zero;  // Reiniciar la velocidad horizontal al aterrizar
        }

        // Movimiento básico
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Determinar si está corriendo (al presionar Shift o tecla especial "C" si tiene la habilidad)
        currentSpeed = walkSpeed;
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift) && canRun)
            {
                currentSpeed = walkSpeed * specialRunMultiplier;
            }
            else
            {
                currentSpeed = walkSpeed;
            }
        }

        if (isGrounded && direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Mover al personaje si está en el suelo
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            horizontalVelocity = moveDir.normalized * currentSpeed; // Guardar la velocidad horizontal al saltar
        }

        // Salto
        if (isGrounded && Input.GetButtonDown("Jump") && canPerformJump)  // Solo salta si tiene la habilidad de Saltar
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Mantener la velocidad horizontal durante el salto
        if (!isGrounded)
        {
            controller.Move(horizontalVelocity * Time.deltaTime);  // Mover horizontalmente en el aire
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }



    private void ManejarAtaque()
    {
        // Habilidad de ataque
        if (canAttack && Input.GetKeyDown(KeyCode.F))
        {
            
        }
    }



    // Método para actualizar los parámetros de animación
    private void UpdateAnimationParameters(Vector3 direction, float currentSpeed)
    {
        // Si la magnitud del vector es cercana a 0, estamos en idle
        if (direction.magnitude < 0.1f)
        {
            anim.SetFloat("speed", 0f);  // Animación de idle
        }
        else if (currentSpeed == walkSpeed)
        {
            anim.SetFloat("speed", 0.5f);  // Animación de caminar
        }
        else if (currentSpeed >= runSpeed)
        {
            anim.SetFloat("speed", 1f);  // Animación de correr
        }

        // Actualizar el estado de si está en el suelo o no
        anim.SetBool("onGround", isGrounded);
    }

    // Método para atacar
    private void Attack()
    {
        anim.SetTrigger("attack"); // Activar animación de ataque
        // Aquí iría la lógica para dañar a enemigos
        Debug.Log("Ataque ejecutado");
    }

    // Método para agregar habilidades al jugador
    public void AddAbility(AbilityType ability)
    {
        switch (ability)
        {
            case AbilityType.Correr:
                canRun = true;
                break;
            case AbilityType.Saltar:
                canPerformJump = true;
                break;
            case AbilityType.Atacar:
                canAttack = true;
                break;
        }
        Debug.Log("Habilidad adquirida: " + ability);
    }

}

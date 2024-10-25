using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    // Movimiento
    public CharacterController controller;
    public Transform cam;

    public float walkSpeed = 6f;  // Velocidad normal
    public float runSpeed = 12f;  // Velocidad al correr

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

    static public bool canJump = false;

    //Animacion
    public Animator anim;


    // Update is called once per frame
    void Update()
    {
        // Comprobar si está en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reiniciar la velocidad en Y cuando toca el suelo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Se asegura de que no siga cayendo infinitamente
            horizontalVelocity = Vector3.zero;  // Reiniciar la velocidad horizontal al aterrizar
        }

        // Movimiento básico
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Determinar si está corriendo (al presionar Shift)
        float currentSpeed = walkSpeed;
        if (isGrounded)
        {
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
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
        if (isGrounded && Input.GetButtonDown("Jump") && canJump)
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

        // Animación
        UpdateAnimationParameters(direction, currentSpeed);

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
        else if (currentSpeed == runSpeed)
        {
            anim.SetFloat("speed", 1f);  // Animación de correr
        }

        // Actualizar el estado de si está en el suelo o no
        anim.SetBool("onGround", isGrounded);
    }

}

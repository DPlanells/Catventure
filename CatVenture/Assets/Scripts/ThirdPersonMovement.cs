using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    // Movimiento
    public CharacterController controller;
    public Transform cam;

    public static float walkSpeed = 6f;  // Velocidad normal
    private static float specialRunMultiplier = 2f;  // Multiplicador de velocidad especial para habilidad Correr
    public float runSpeed = walkSpeed * specialRunMultiplier;  // Velocidad al correr
    public float currentSpeed;

    private Vector3 direction;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Salto
    public float jumpHeight = 2f;  // Altura del salto
    public float gravity = -22f;  // Gravedad aplicada
    private Vector3 velocity;  // Velocidad para el movimiento vertical
    private Vector3 horizontalVelocity;  // Velocidad horizontal en el salto

    public Transform groundCheck;  // Punto para verificar si está en el suelo
    public float groundDistance = 3f;  // Distancia del punto de verificación del suelo
    public LayerMask groundMask;  // Capa que representa el suelo

    bool isGrounded;
    bool isDancing;
    bool isAttacking;
    bool hasDoubleJumped;


    //Ataque
    public float attackRange = 2f; // Rango del ataque
    public float attackAngle = 45f; // Ángulo del ataque
    public int attackDamage = 1; // Daño del ataque
    public Transform attackPoint; // Punto desde donde se realiza el ataque
    public float attackCooldown = 0.5f; // Tiempo de enfriamiento entre ataques





    // Animación
    public Animator anim;

    // Habilidades
    //public enum AbilityType { Correr, Saltar, Atacar }
    public bool canRun = false;
    public bool canJump = false;
    public bool canAttack = false;
    public bool canLaunch = false;
    public bool canDoubleJump = false;


    // Lanzarse
    private float launchDuration = 1f; // Duración máxima de la acción de lanzarse
    private float launchAngle = 0f; // Ángulo hacia abajo en grados
    private bool isLaunching = false;
    private float launchTimeRemaining;

    //Tiempo de invulnerabilidad
    public float invulnerabilityDuration = 2f; // Duración de la invulnerabilidad en segundos
    private bool isInvulnerable = false; // Estado de invulnerabilidad
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        ManejarMovimiento();
        ManejarAtaque();
        ManejarLanzarse();
        // Animación
        UpdateAnimationParameters(direction, currentSpeed);

    }

    void FixedUpdate()
    {
        isGrounded =  Physics.CheckSphere(groundCheck.position,groundDistance ,groundMask);
    }
    private void ManejarMovimiento()
    {
        if (isLaunching) return; // No manejar movimiento normal si está lanzándose

        // Reiniciar la velocidad en Y cuando toca el suelo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;  // Asegura que no siga cayendo infinitamente
            horizontalVelocity = Vector3.zero;  // Reiniciar la velocidad horizontal al aterrizar
            hasDoubleJumped = false; // Reiniciar el estado del doble salto al tocar el suelo
        }

        // Movimiento básico
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Determinar si está corriendo (al presionar Shift o tecla especial "C" si tiene la habilidad)
        currentSpeed = walkSpeed;
        if (isGrounded)
        {
            if (Input.GetButton("Run") && canRun)
            {
                currentSpeed = runSpeed;
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
            isDancing = false;
        }

        // Salto
        if (Input.GetButtonDown("Jump") && canJump)  // Solo salta si tiene la habilidad de Saltar
        {
            if (isGrounded) // Primer salto
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isDancing = false;
            }
            else if (canDoubleJump && !hasDoubleJumped) // Doble salto
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                hasDoubleJumped = true; // Marcar que ya se usó el doble salto
                isDancing = false;
            }
        }

        // Baile
        if (isGrounded && Input.GetButtonDown("Dance"))
        {
            isDancing = true;
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
        if (canAttack && Input.GetButtonDown("Attack"))
        {
            Attack();
            isAttacking = true;
        }
    }


    private void Attack()
    {
        // Inicia el cooldown del ataque
        StartCoroutine(AttackCooldown());

        // Obtiene todos los colliders en un radio del punto de ataque
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach (Collider collider in hitColliders)
        {
            // Verifica si el objeto tiene el tag "Enemy"
            if (collider.CompareTag("Enemy"))
            {
                // Calcula la dirección hacia el enemigo
                Vector3 directionToEnemy = collider.transform.position - transform.position;
                directionToEnemy.y = 0; // Ignorar diferencia en altura

                // Verifica si el enemigo está dentro del ángulo de ataque
                if (Vector3.Angle(transform.forward, directionToEnemy) <= attackAngle)
                {
                    // Aplica daño al enemigo (se asume que el enemigo tiene un script con un método "TakeDamage")
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(attackDamage);
                    }
                }
            }
        }


    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        canAttack = false; // Desactiva la capacidad de atacar
        yield return new WaitForSeconds(attackCooldown); // Espera el tiempo del cooldown
        canAttack = true; // Reactiva la capacidad de atacar
        isAttacking = false;
    }


    private void ManejarLanzarse()
    {
        if (Input.GetButtonDown("Dive") && !isGrounded && !isLaunching && canLaunch)
        {
            isLaunching = true;
            launchTimeRemaining = launchDuration;

            // Calcular dirección del lanzamiento (20º hacia abajo desde la orientación actual)
            Vector3 launchDirection = Quaternion.Euler(-launchAngle, 0f, 0f) * transform.forward;

            // Aplicar impulso inicial
            velocity.y = 0; // Reiniciar cualquier velocidad vertical previa
            horizontalVelocity = launchDirection.normalized * runSpeed;
        }

        if (isLaunching)
        {
            // Reducir el tiempo restante del lanzamiento
            launchTimeRemaining -= Time.deltaTime;

            // Mover al jugador en la dirección calculada
            controller.Move(horizontalVelocity * Time.deltaTime);

            // Gradualmente reducir la velocidad horizontal
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime / launchDuration);

            // Terminar el lanzamiento si se acaba el tiempo
            if (launchTimeRemaining <= 0f)
            {
                TerminarLanzarse();
            }
        }
    }

    private void TerminarLanzarse()
    {
        isLaunching = false;

        // Mantener la velocidad horizontal restante para movimiento en el aire
        if (!isGrounded)
        {
            velocity.y = 0; // Reiniciar la velocidad vertical para permitir caída normal
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

        anim.SetBool("Dance", isDancing);

        anim.SetBool("Attack", isAttacking);

        anim.SetBool("Launching", isLaunching);
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
                canJump = true;
                break;
            case AbilityType.Atacar:
                canAttack = true;
                break;
        }
        Debug.Log("Habilidad adquirida: " + ability);
    }

    public void setRun(bool run)
    {
        canRun = run;
    }

    public void setJump(bool jump)
    {
        canJump = jump;
    }

    public void setAttack(bool attack)
    {
        canAttack = attack;
    }

    public void setLaunch(bool launch)
    {
        canLaunch = launch;
    }

    public void setDoubleJump(bool doubleJump)
    {
        canDoubleJump = doubleJump;
    }

    public bool getRun()
    {
        return canRun;
    }

    public bool getJump()
    {
        return canJump;
    }

    public bool getAttack()
    {
        return canAttack;
    }

    public bool getLaunch()
    {
        return canLaunch;
    }

    public bool getDoubleJump()
    {
        return canDoubleJump;
    }

    public void activarVulnerabilidad()
    {
        // Activar el estado de invulnerabilidad
        StartCoroutine(InvulnerabilityTimer());
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true; // Activar invulnerabilidad


        yield return new WaitForSeconds(invulnerabilityDuration); // Esperar el tiempo de invulnerabilidad

        isInvulnerable = false; // Desactivar invulnerabilidad

    }

    public bool getVulnerable()
    {
        return isInvulnerable;
    }

    public void setPosicion(Vector3 posicion)
    {
        transform.position = posicion;
    }



    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Se ha chocado con: " + collision.gameObject.name);

       
        if (collision.gameObject.CompareTag("AguaFondo")){
            GameManager.instance.dañarJugadorAgua(3);
            Debug.Log("Se han quitado 3 vidas");
        }
        
    }

    



}

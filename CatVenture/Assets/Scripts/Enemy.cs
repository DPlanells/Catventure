using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 2; // Salud máxima del enemigo
    private int currentHealth;

    void Start()
    {
        // Inicializa la salud del enemigo
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Reduce la salud actual
        currentHealth -= damage;

        // Muestra un mensaje en consola para depuración
        Debug.Log(gameObject.name + " tomó " + damage + " puntos de daño. Salud restante: " + currentHealth);

        // Comprueba si la salud llega a 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Muestra un mensaje en consola para depuración
        Debug.Log(gameObject.name + " ha muerto.");

        // Desactiva o destruye el enemigo
        Destroy(gameObject); // Puedes reemplazar esto con una animación de muerte o lógica personalizada
    }
}


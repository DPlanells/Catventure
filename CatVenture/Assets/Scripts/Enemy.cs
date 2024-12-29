using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 2; // Salud m�xima del enemigo
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

        // Muestra un mensaje en consola para depuraci�n
        Debug.Log(gameObject.name + " tom� " + damage + " puntos de da�o. Salud restante: " + currentHealth);

        // Comprueba si la salud llega a 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Muestra un mensaje en consola para depuraci�n
        Debug.Log(gameObject.name + " ha muerto.");

        // Desactiva o destruye el enemigo
        Destroy(gameObject); // Puedes reemplazar esto con una animaci�n de muerte o l�gica personalizada
    }
}


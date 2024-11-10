using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;   // Daño que causa al jugador


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            GameManager.instance.dañarJugador(damage);
            Destroy(gameObject);
        }
    }

}

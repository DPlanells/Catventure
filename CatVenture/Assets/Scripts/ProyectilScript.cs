using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;   // Da�o que causa al jugador


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            GameManager.instance.da�arJugador(damage);
            Destroy(gameObject);
        }
    }

}

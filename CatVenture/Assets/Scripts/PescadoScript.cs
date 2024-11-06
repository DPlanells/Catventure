using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pescado : MonoBehaviour
{

    
    public AbilityType abilityType;  // Selecciona la habilidad a otorgar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // Otorgar la habilidad seleccionada al jugador
            GameManager.instance.AddAbility(abilityType);
                
            // Destruir el coleccionable tras ser recogido
            Destroy(gameObject);
        }
    }
    

}

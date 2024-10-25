using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public static event Action OnCollected;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, Time.time * 100f,0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdPersonMovement.canJump = true;
            OnCollected?.Invoke();
            Destroy(gameObject);
        }
    }

}

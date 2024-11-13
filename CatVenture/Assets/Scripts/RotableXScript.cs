using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotableXScript : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de rotación en grados por segundo

    private void Update()
    {
        // Rotar en el eje X en sentido de las agujas del reloj
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

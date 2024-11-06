using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float floatAmplitude = 0.25f;   // Amplitud del movimiento de flotaci�n (cu�nto sube y baja)
    public float floatSpeed = 0.5f;       // Velocidad de flotaci�n (qu� tan r�pido sube y baja)

    private Vector3 startPos;

    private void Start()
    {
        // Guardar la posici�n inicial del objeto
        startPos = transform.position;
    }

    private void Update()
    {
        // Calcular el nuevo desplazamiento en el eje Y
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Aplicar la nueva posici�n
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

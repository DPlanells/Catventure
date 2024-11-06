using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float floatAmplitude = 0.25f;   // Amplitud del movimiento de flotación (cuánto sube y baja)
    public float floatSpeed = 0.5f;       // Velocidad de flotación (qué tan rápido sube y baja)

    private Vector3 startPos;

    private void Start()
    {
        // Guardar la posición inicial del objeto
        startPos = transform.position;
    }

    private void Update()
    {
        // Calcular el nuevo desplazamiento en el eje Y
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Aplicar la nueva posición
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

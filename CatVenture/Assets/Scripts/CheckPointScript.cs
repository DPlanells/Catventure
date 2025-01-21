using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public Renderer sphereRenderer;  // Referencia al Renderer de la Sphere
    public Material amarilloSolido; // Material amarillo
    public Light farolaLight;       // Referencia a la luz

    public Transform puntoSpawn;
    private Vector3 coordenadas; //Coordenadas del punto de spawn

    private bool isActivated = false; // Estado de la farola

    void OnTriggerEnter(Collider other)
    {
        // Comprueba si el jugador ha chocado
        if (!isActivated && other.CompareTag("Player"))
        {
            // Cambia el material de la Sphere
            sphereRenderer.material = amarilloSolido;

            // Activa la luz
            farolaLight.enabled = true;

            // Cambia el estado a activado
            isActivated = true;


            coordenadas = puntoSpawn.position;
            GameManager.instance.setCheckPoint(coordenadas);

            // Guardar el progreso del juego
            GameManager.instance.SaveProgress(GameManager.instance.getSlotGuardado());

            Debug.Log("Se ha guardado el checkpoint y el progreso del juego.");

        }
    }
}

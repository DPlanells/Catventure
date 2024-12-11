using UnityEngine;
using UnityEngine.UI;

public class CanvasJuego : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    public int lives = 0; // Número inicial de vidas.
    public float marginX = 10f; // Margen desde el borde izquierdo del Canvas.
    public float marginY = 10f; // Margen desde el borde superior del Canvas.
    public float spacing = 2f; // Espaciado entre imágenes de corazones.

    [Header("Configuración de Corazón")]
    public Sprite heartSprite; // Sprite del corazón (asignado en el Inspector).

    private RectTransform canvasTransform;

    void Start()
    {
        // Obtener el RectTransform del Canvas.
        canvasTransform = GetComponent<RectTransform>();

        if (canvasTransform == null)
        {
            Debug.LogError("Este script debe colocarse en un objeto Canvas.");
            return;
        }

        // Generar los corazones iniciales.
        GenerateHearts();
    }

    void GenerateHearts()
    {
        // Eliminar cualquier corazón previo (si los hay).
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Crear tantas imágenes como vidas tenga el jugador.
        for (int i = 0; i < lives; i++)
        {
            // Crear un nuevo GameObject para cada corazón.
            GameObject heart = new GameObject("Heart" + i, typeof(Image));

            // Configurar su posición y tamaño dentro del Canvas.
            RectTransform rectTransform = heart.GetComponent<RectTransform>();
            rectTransform.SetParent(canvasTransform);

            // Usar el tamaño del sprite para definir el tamaño del RectTransform.
            if (heartSprite != null)
            {
                rectTransform.sizeDelta = new Vector2(heartSprite.rect.width, heartSprite.rect.height);
            }
            else
            {
                Debug.LogWarning("No se asignó un sprite para los corazones.");
            }

            // Posicionar en la esquina superior izquierda con márgenes.
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(marginX + i * (rectTransform.sizeDelta.x + spacing), -marginY);

            // Asignar el sprite al componente Image.
            Image image = heart.GetComponent<Image>();
            image.sprite = heartSprite;
        }
    }

    void Update()
    {
        if (lives != GameManager.instance.getVidas())
        {
            lives = GameManager.instance.getVidas();
            GenerateHearts();
        }
    }
}


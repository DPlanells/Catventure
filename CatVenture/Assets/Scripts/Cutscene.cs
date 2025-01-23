using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Cutscene : MonoBehaviour
{
    public Canvas Canvas1;
    public Canvas Canvas2;
    public Image Viñeta1;
    public Image Viñeta2;
    public Image Viñeta3;
    public Image Viñeta4;
    public Image Viñeta5;
    public Image Viñeta6;
    private int contador = 1;

    public void Boton()
    {
        contador = contador + 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        Canvas2.enabled = false;
        Viñeta1.enabled = true;
        Viñeta2.enabled = false;
        Viñeta3.enabled = false;
        Viñeta4.enabled = false;
        Viñeta5.enabled = false;
        Viñeta6.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (contador) {
            case 1: break;
            case 2:
                Viñeta2.enabled = true;
                break;
            case 3:
                Viñeta3.enabled = true;
                break;
            case 4:
                Viñeta4.enabled = true;
                break;
            case 5:
                Canvas1.enabled = false;
                Canvas2.enabled = true;
                Viñeta5.enabled = true;
                break;
            case 6:
                Viñeta6.enabled = true;
                break;
            case 7:
                SceneManager.LoadScene("MainScene");
                break;
            default: break;
        }

      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Fin : MonoBehaviour
{
    public Canvas Canvas1;
    public Canvas Canvas2;
    public Image Vi�eta1;
    public Image Vi�eta2;
    public Image Vi�eta3;
    public Image Vi�eta4;
    private int contador = 1;

    public void Boton()
    {
        contador = contador + 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        Canvas2.enabled = false;
        Vi�eta1.enabled = true;
        Vi�eta2.enabled = false;
        Vi�eta3.enabled = false;
        Vi�eta4.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        switch (contador) {
            case 1: break;
            case 2:
                Vi�eta2.enabled = true;
                break;
            case 3:
                Vi�eta3.enabled = true;
                break;
            case 4:
                Canvas1.enabled = false;
                Canvas2.enabled = true;
                Vi�eta4.enabled = true;
                break;
            case 5:
                SceneManager.LoadScene("Sandbox Menu");
                break;
            default: break;
        }

      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CambiarTexto : MonoBehaviour
{
    public TMP_Text textoVidas; // Asigna el objeto de texto en el Inspector
    public TMP_Text textoMonedas;

    private void Start()
    {
        CambiarTextos();
    }

    private void Update()
    {
        CambiarTextos();
    }

    // Método para cambiar el texto
    public void CambiarTextos()
    {

        textoVidas.text = GameManager.instance.getVidas().ToString();
        textoMonedas.text = GameManager.instance.getCroquetas().ToString();
    }
}

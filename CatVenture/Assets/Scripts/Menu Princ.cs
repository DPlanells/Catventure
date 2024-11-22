using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrinc : MonoBehaviour
{

    public Animator MCamera;

    private void Start()
    {
        MCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }
// Start is called before the first frame update
    public void NuevaPartida()

    {
    SceneManager.LoadScene("MainScene");
    }
    public void CargarPartida()
    {
        Debug.Log(MCamera.gameObject);
        MCamera.Play("Continuar");
    }
    public void Creditos()
    {
        Debug.Log(MCamera.gameObject);
        MCamera.Play("Creditos"); 
    }

}

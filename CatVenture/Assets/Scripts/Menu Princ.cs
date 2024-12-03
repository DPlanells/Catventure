using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrinc : MonoBehaviour
{

    public Animator MCamera;
    public Animator MUIChild1;
    public Animator MUIChild2;
    public Animator MUIChild3;
    public Canvas MUI;
    public Canvas CredUI;
    public Animator CredAnim;

    private void Start()
    {
        MCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        MUI = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        CredUI = GameObject.FindGameObjectWithTag("Creditos").GetComponent<Canvas>();
        MCamera.SetBool("Creditos", false);
        CredUI.enabled = false;

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
        
        MUI.enabled = false;
        MCamera.SetBool("Creditos", true);
        CredUI.enabled = true;
        CredAnim.SetTrigger("creditos");


    }
    public void VolverPrincipal()
    {
        MCamera.SetBool("Creditos", false);
        CredAnim.SetTrigger("creditosvolver");
        CredUI.enabled = false;
        Debug.Log(MUIChild1);
        MUIChild1.SetTrigger("trns");
        MUIChild2.SetTrigger("trns");
        MUIChild3.SetTrigger("trns");
        MUI.enabled = true;
    }

}

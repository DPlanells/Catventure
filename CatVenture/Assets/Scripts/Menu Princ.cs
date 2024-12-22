using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrinc : MonoBehaviour
{

    public Animator MCamera;
    public Animator MUIChild2;
    public Animator MUIChild3;
    public Animator MUIChild4;
    public Canvas MUI;
    public Canvas CredUI;
    public Animator CredAnim;
    public Animator BotonVolver;
    public Button IniciarBoton;
    public Button VolverBoton;
    int tiempo;

    private void Start()
    {
        MCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        MUI = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        CredUI = GameObject.FindGameObjectWithTag("Creditos").GetComponent<Canvas>();

        
        MCamera.SetBool("Creditos", false);
        CredUI.enabled = false;

    }

    public void Update() {
        bool MUIBool = MUI.enabled;
        if (Input.GetKeyDown(KeyCode.Return) && MUIBool) { 
            IniciarBoton.onClick.Invoke(); 
        }
        bool CredBool = CredUI.enabled;
        if (Input.GetKeyDown(KeyCode.Escape) && CredBool)
        {
            VolverBoton.onClick.Invoke();
        }
    }

    public void CoroutineTimer(int tiempo)
    {
        
        StartCoroutine(timer(tiempo));
    }

    private IEnumerator timer(int tiempo)
    {
        yield return new WaitForSeconds(tiempo/2);
        MUI.enabled = false;
        yield return new WaitForSeconds(tiempo);
        SceneManager.LoadScene("MainScene");

    }

    public void NuevaPartida()

    {
        MCamera.SetTrigger("IniciarPartida");
        tiempo = 3;
        CoroutineTimer(tiempo);
        
        
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
        BotonVolver.SetTrigger("creditos");
        CredAnim.SetTrigger("creditos");


    }
    public void VolverPrincipal()
    {
        MCamera.SetBool("Creditos", false);
        CredAnim.SetTrigger("creditosvolver");
        CredUI.enabled = false;
        Debug.Log(MUIChild2);
        MUIChild2.SetTrigger("trns");
        MUIChild3.SetTrigger("trns");
        MUIChild4.SetTrigger("trns");
        MUI.enabled = true;
    }

    public void salirJuego()
    {
        Application.Quit();
    }
}

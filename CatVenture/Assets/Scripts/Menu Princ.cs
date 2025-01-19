using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Canvas CargarUI;
    public Animator CredAnim;
    public Animator BotonVolver;
    public Button IniciarBoton;
    public Button VolverBoton;
    public Animator Partida1;
    public Animator Partida2;
    public Animator Partida3;
    int tiempo;
    private int Slot = 1;



    private void Start()
    {
        MCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        MUI = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        CredUI = GameObject.FindGameObjectWithTag("Creditos").GetComponent<Canvas>();
        CargarUI = GameObject.FindGameObjectWithTag("Cargar").GetComponent<Canvas>();

        MCamera.SetBool("Creditos", false);
        CredUI.enabled = false;
        CargarUI.enabled = false;


    }

    public void SeleccionarSlot(int slot)
    {
        Slot = slot;

        // Guarda el slot seleccionado temporalmente en PlayerPrefs o en un singleton temporal
        PlayerPrefs.SetInt("SlotSeleccionado", Slot);
        PlayerPrefs.Save();

        // Cambia a la escena principal
        SceneManager.LoadScene("MainScene");
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
        SeleccionarSlot(Slot);

    }

    public void NuevaPartida()

    {
        MCamera.SetTrigger("IniciarPartida");
        tiempo = 3;
        CoroutineTimer(tiempo);
        
        
    }
    public void CargarPartida()
    {
        MCamera.SetBool("Cargar", true);
        CargarUI.enabled = true;
        CredUI.enabled = false;
        MUI.enabled = false;
        MCamera.Play("Continuar");
        Partida1.SetTrigger("trns");
        Partida2.SetTrigger("trns");
        Partida3.SetTrigger("trns");
    }
    public void Creditos()
    {
        Debug.Log(MCamera.gameObject);
        CargarUI.enabled = false;
        MUI.enabled = false;
        MCamera.SetBool("Creditos", true);
        CredUI.enabled = true;
        BotonVolver.SetTrigger("creditos");
        CredAnim.SetTrigger("creditos");


    }
    public void VolverPrincipal()
    {

        if (CredUI.enabled)
        {
            MCamera.SetBool("Creditos", false);
            CredAnim.SetTrigger("creditosvolver");
            CredUI.enabled = false;
        }
        else if (CargarUI.enabled)
        {
            MCamera.SetBool("Cargar", false);
            CargarUI.enabled = false;
        }
        MUIChild2.SetTrigger("trns");
        MUIChild3.SetTrigger("trns");
        MUIChild4.SetTrigger("trns");
        MUI.enabled = true;
    }

    public void salirJuego()
    {
        Application.Quit();
    }

    private SaveData getInfoSlot(int slot)
    {
        SaveManager saveManager = GameObject.FindObjectOfType<SaveManager>();
        if (saveManager == null)
        {
            Debug.Log("No se ha encontrado el componente saveManager");
            return null;
        }
        else
        {
            SaveData info = saveManager.LoadGame(slot);
            return info;
        }
    }
}

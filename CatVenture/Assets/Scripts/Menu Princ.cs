using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuPrinc : MonoBehaviour
{

    public Animator MCamera;
    public Animator MUIChild2;
    public Animator MUIChild3;
    public Animator MUIChild4;
    public Canvas MUI;
    public Canvas CredUI;
    public Canvas CargarUI;
    public Canvas ElegirUI;
    public Animator CredAnim;
    public Animator BotonVolver;
    public Button IniciarBoton;
    public Button botonslot1;
    public Button botonslot2;
    public Button botonslot3;
    public Button elegirslot1;
    public Button elegirslot2;
    public Button elegirslot3;
    public Button VolverBoton;
    public Animator Partida1;
    public Animator Partida2;
    public Animator Partida3;
    int tiempo;
    //private int Slot = 2;
    private int Slot;
    private int livesslot1;
    private int livesslot2;
    private int livesslot3;
    private int coinsslot1;
    private int coinsslot2;
    private int coinsslot3;
    private int liveselegir1;
    private int liveselegir2;
    private int liveselegir3;
    private int coinselegir1;
    private int coinselegir2;
    private int coinselegir3;
    private SaveData info1;
    private SaveData info2;
    private SaveData info3;
    public TMP_Text UIVidasText1;
    public TMP_Text UIVidasText2;
    public TMP_Text UIVidasText3;
    public TMP_Text UICoinsText1;
    public TMP_Text UICoinsText2;
    public TMP_Text UICoinsText3;
    public TMP_Text UIVidasElegirText1;
    public TMP_Text UIVidasElegirText2;
    public TMP_Text UIVidasElegirText3;
    public TMP_Text UICoinsElegirText1;
    public TMP_Text UICoinsElegirText2;
    public TMP_Text UICoinsElegirText3;

    private void Start()
    {
        MCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        MUI = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>();
        CredUI = GameObject.FindGameObjectWithTag("Creditos").GetComponent<Canvas>();
        CargarUI = GameObject.FindGameObjectWithTag("Cargar").GetComponent<Canvas>();
        ElegirUI = GameObject.FindGameObjectWithTag("Elegir").GetComponent<Canvas>();

        MCamera.SetBool("Creditos", false);
        enableAll(CredUI, false);
        enableAll(CargarUI, false);
        enableAll(ElegirUI, false);

    }

    public void SeleccionarSlot(int slot)
    {
        Slot = slot;

        // Guarda el slot seleccionado temporalmente en PlayerPrefs o en un singleton temporal
        PlayerPrefs.SetInt("SlotSeleccionado", Slot);
        PlayerPrefs.Save();

        // Cambia a la escena principal
        NuevaPartida();
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
        yield return new WaitForSeconds(tiempo / 2);
        MUI.enabled = false;
        yield return new WaitForSeconds(tiempo);
        SceneManager.LoadScene("Menu Principal");

    }

    public void NuevaPartida()

    {
        MCamera.SetTrigger("IniciarPartida");
        tiempo = 3;
        CoroutineTimer(tiempo);


    }
    public void CargarPartida()
    {
        SaveManager saveManager = GameObject.FindObjectOfType<SaveManager>();
        info1 = getInfoSlot(1);
        info2 = getInfoSlot(2);
        info3 = getInfoSlot(3);
        if (saveManager.SaveSlotExists(1)) {
            UIVidasText1.text = info1.lives.ToString();
            UICoinsText1.text = info1.coins.ToString();
            
        } else { botonslot1.interactable = false; }
        if (saveManager.SaveSlotExists(2)) {
            UIVidasText2.text = info2.lives.ToString();
            UICoinsText2.text = info2.coins.ToString();
            UIVidasElegirText2.text = info2.lives.ToString();
            UICoinsElegirText2.text = info2.coins.ToString();
        } else { botonslot2.interactable = false; }
        if (saveManager.SaveSlotExists(3)) {
            UIVidasText3.text = info3.lives.ToString();
            UICoinsText3.text = info3.coins.ToString();
        } else { botonslot3.interactable = false; }
        MCamera.SetBool("Cargar", true);
        ElegirUI.enabled = false;
        enableAll(ElegirUI, false);
        CargarUI.enabled = true;
        enableAll(CargarUI, true);
        CredUI.enabled = false;
        enableAll(CredUI, false);
        MUI.enabled = false;
        enableAll(MUI, false);
        MCamera.Play("Continuar");
        Partida1.SetTrigger("trns");
        Partida2.SetTrigger("trns");
        Partida3.SetTrigger("trns");
    }

    public void ElegirSlot()
    {
        SaveManager saveManager = GameObject.FindObjectOfType<SaveManager>();
        info1 = getInfoSlot(1);
        info2 = getInfoSlot(2);
        info3 = getInfoSlot(3);
        if (saveManager.SaveSlotExists(1))
        {
            UIVidasElegirText1.text = info1.lives.ToString();
            UICoinsElegirText1.text = info1.coins.ToString();
            elegirslot1.interactable = false;
        }
        else { botonslot1.interactable = true; }
        if (saveManager.SaveSlotExists(2))
        {
            UIVidasElegirText2.text = info2.lives.ToString();
            UICoinsElegirText2.text = info2.coins.ToString();
            elegirslot2.interactable = false;
        }
        else { botonslot2.interactable = true; }
        if (saveManager.SaveSlotExists(3))
        {
            UIVidasElegirText3.text = info3.lives.ToString();
            UICoinsElegirText3.text = info3.coins.ToString();
            elegirslot3.interactable = false;
        }
        else { botonslot3.interactable = true; }
        MCamera.SetBool("Elegir", true);
        CargarUI.enabled = false;
        enableAll(CargarUI, false);
        ElegirUI.enabled = true;
        enableAll(ElegirUI, true);
        CredUI.enabled = false;
        enableAll(CredUI, false);
        MUI.enabled = false;
        enableAll(MUI, false);
        Partida1.SetTrigger("trns");
        Partida2.SetTrigger("trns");
        Partida3.SetTrigger("trns");
    }

    public void Creditos()
    {
        Debug.Log(MCamera.gameObject);
        ElegirUI.enabled = false;
        enableAll(ElegirUI, false);
        CargarUI.enabled = false;
        enableAll(CargarUI, false);
        MUI.enabled = false;
        enableAll(MUI, false);
        MCamera.SetBool("Creditos", true);
        enableAll(CredUI, true);
        BotonVolver.SetTrigger("creditos");
        CredAnim.SetTrigger("creditos");
        CredUI.enabled = true;

    }
    public void VolverPrincipal()
    {

        if (CredUI.enabled)
        {
            MCamera.SetBool("Creditos", false);
            CredAnim.SetTrigger("creditosvolver");
            CredUI.enabled = false;
            enableAll(CredUI, false);


        }
        else if (ElegirUI.enabled)
        {
            ElegirUI.enabled = false;
            MCamera.SetBool("Elegir", false);
            enableAll(ElegirUI, false);
        }
        else if (CargarUI.enabled)
        {
            CargarUI.enabled = false;
            MCamera.SetBool("Cargar", false);
            enableAll(CargarUI, false);
        }
        MUIChild2.SetTrigger("trns");
        MUIChild3.SetTrigger("trns");
        MUIChild4.SetTrigger("trns");
        MUI.enabled = true;
        enableAll(MUI, true);
    }

    public void salirJuego()
    {
        Application.Quit();
    }

    public SaveData getInfoSlot(int slot)
    {
        SaveManager saveManager = GameObject.FindObjectOfType<SaveManager>();
        if (saveManager == null)
        {
            Debug.Log("No se ha encontrado el componente saveManager");
            return null;
        }
        else
        {
            Debug.Log("cargado");
            SaveData info = saveManager.LoadGame(slot);
            return info;
        }
    }

    public void enableAll(Canvas objeto, bool enable)
    {
        for (int i = 0; i < objeto.transform.childCount; i++)
        {
            var child = objeto.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(enable);
        }
    }
}

using System.IO;
using UnityEngine;

public class GameLoader : MonoBehaviour
{

    private SaveManager saveManager;

    private void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
    }

    public SaveData LoadProgress(int slot)
    {
        SaveData data = saveManager.LoadGame(slot);
        if (data == null)
        {
            Debug.LogWarning($"No se pudo cargar el progreso para el slot {slot}. Archivo inexistente o corrupto.");
            return null;
        }

        return data;
    }

}


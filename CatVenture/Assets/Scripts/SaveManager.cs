using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const int MaxSaveSlots = 3;
    private string saveFolderPath;


    private void Awake()
    {
        InitializeSaveFolderPath();
    }

    public void crearGuardadoPrueba()
    {
        SaveData Prueba = new SaveData();
        Prueba.lives = 5;
        Prueba.coins = 5;
        Prueba.canJump = true;
        Prueba.canRun = true;
        Prueba.canDoubleJump = false;
        Prueba.canLaunch = false;
        Prueba.canAttack = false;
        Prueba.checkpointPosition = new Vector3(870, 60, 560);

        SaveGame(Prueba, 2);
    }

    private void InitializeSaveFolderPath()
    {
        if (string.IsNullOrEmpty(saveFolderPath))
        {
            saveFolderPath = Application.persistentDataPath + "/Saves";
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }
        }
    }

    private string GetSaveFilePath(int slot)
    {
        InitializeSaveFolderPath(); // Asegurar que la ruta esté inicializada
        return Path.Combine(saveFolderPath, $"save_slot_{slot}.json");
    }

    public void SaveGame(SaveData data, int slot)
    {
        if (slot < 1 || slot > MaxSaveSlots)
        {
            Debug.LogError("Slot fuera de rango. Debe estar entre 1 y " + MaxSaveSlots);
            return;
        }

        string filePath = GetSaveFilePath(slot);
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Juego guardado en el slot " + slot);
    }



    public SaveData LoadGame(int slot)
    {
        if (slot < 1 || slot > MaxSaveSlots)
        {
            Debug.LogError("Slot fuera de rango. Debe estar entre 1 y " + MaxSaveSlots);
            return null;
        }

        string filePath = GetSaveFilePath(slot);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado en el slot " + slot);
            return null;
        }
    }

    public bool SaveSlotExists(int slot)
    {
        return File.Exists(GetSaveFilePath(slot));
    }

}

using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const int MaxSaveSlots = 3;
    private string saveFolderPath;

    private void Awake()
    {
        saveFolderPath = Application.persistentDataPath + "/Saves";
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
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

    private string GetSaveFilePath(int slot)
    {
        return Path.Combine(saveFolderPath, $"save_slot_{slot}.json");
    }
}

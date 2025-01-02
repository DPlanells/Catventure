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


        return null;
    }
}


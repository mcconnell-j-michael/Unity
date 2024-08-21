using UnityEngine;
using System.Collections;
using TMPro;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public TMP_InputField saveName;
    public GameObject loadButtonPrefab;
    public SerializationManager serializationManager;

    public void Start()
    {
        serializationManager = GetComponent<SerializationManager>();
    }

    public void OnSave()
    {
        serializationManager.Save($"{Application.persistentDataPath}/saves/{saveName.text}");
    }

    public string[] saveFiles;

    public void GetLoadFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }

        saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");
    }
}

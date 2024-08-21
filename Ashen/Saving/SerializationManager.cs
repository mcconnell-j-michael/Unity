using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SerializationManager : MonoBehaviour
{
    private string savePath;
    private string SavePath
    {
        get
        {
            if (savePath == null)
            {
                savePath = $"{Application.persistentDataPath}/saves/save.txt";
            }
            return savePath;
        }
    }

    [Button]
    private void SaveDefault()
    {
        Save(SavePath);
    }

    [Button]
    private void LoadDefault()
    {
        load(SavePath);
    }

    public void Save(string savePath)
    {
        Dictionary<string, object> state = new Dictionary<string, object>();//LoadFile(savePath);
        CaptureState(state);
        SaveFile(savePath, state);
    }

    public void load(string savePath)
    {
        Dictionary<string, object> state = LoadFile(savePath);
        RestoreState(state);
    }

    private void SaveFile(string savePath, object state)
    {
        using (FileStream stream = File.Open(savePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private Dictionary<string, object> LoadFile(string savePath)
    {
        if (!File.Exists(savePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(savePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}

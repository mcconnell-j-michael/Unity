using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class SaveableEntity : A_SavableObject
{
    [SerializeField]
    private string id = string.Empty;

    public string Id
    {
        get
        {
            return id;
        }
    }

    [Button]
    private void GenerateNewId()
    {
        id = Guid.NewGuid().ToString();
    }
}

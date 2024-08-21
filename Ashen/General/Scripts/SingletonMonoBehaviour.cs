using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class SingletonMonoBehaviour<T> : SerializedMonoBehaviour where T : SingletonMonoBehaviour<T>
{

    static T _instance = null;
    public static T Instance
    {
        get
        { 
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        _instance = this as T;
    }
}

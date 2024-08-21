using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/**
 * The TimeManager is used to help the TimeRegister to be passed
 * Unity time updates
 **/
public class TimeManager : MonoBehaviour
{
    public TimeRegistry timeRegistry;
    public List<SerializedScriptableObject> preLoad;

    [ShowInInspector]
    public int Total
    {
        get
        {
            if (timeRegistry)
            {
                return TimeRegistry.GetTotal();
            }
            return 0;
        }
    }

    private void Awake()
    {
        timeRegistry.Clear();
    }

    private void Update()
    {
        timeRegistry.UpdateTime();
    }

    private void OnDestroy()
    {
        timeRegistry.Clear();
    }

    public static bool isLoading = false;

#if UNITY_EDITOR
    [Button]
    public static void LoadAllScriptableObjects()
    {
        if (!isLoading)
        {
            isLoading = true;
            List<SerializedScriptableObject> objects = StaticUtilities.FindAssetsByType<SerializedScriptableObject>();
            isLoading = false;
        }
    }
#endif
}

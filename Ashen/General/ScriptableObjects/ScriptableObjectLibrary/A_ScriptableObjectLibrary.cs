using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_ScriptableObjectLibrary<T, E> : SingletonScriptableObject<T> where T : A_ScriptableObjectLibrary<T, E> where E : ScriptableObject
{
    [OdinSerialize]
    public Dictionary<string, E> library;

    public E GetScriptableObject(string name)
    {
#if UNITY_EDITOR
        LoadSkillNodes();
#endif
        if (library.TryGetValue(name, out E value))
        {
            return value;
        }
        return null;
    }

#if UNITY_EDITOR
    [Button]
    public void LoadSkillNodes()
    {
        if (library == null)
        {
            library = new Dictionary<string, E>();
        }
        List<E> sos = StaticUtilities.FindAssetsByType<E>();
        foreach (E so in sos)
        {
            if (!library.ContainsValue(so))
            {
                library.Add(so.name, so);
            }
        }
    }
#endif
}

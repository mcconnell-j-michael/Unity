using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonScriptableObject<PoolManager>
{
    public GameObject damageText;

    public List<PrefabPool> defaultPrefabPools;

    [NonSerialized]
    private List<PrefabPool> allPrefabPools;

    public PrefabPool GetPoolManager(GameObject prefab, GameObject parent = null)
    {
        if (allPrefabPools == null)
        {
            allPrefabPools = new List<PrefabPool>();
            if (defaultPrefabPools != null)
            {
                allPrefabPools.AddRange(defaultPrefabPools);
            }
        }
        for (int x = 0; x < allPrefabPools.Count; x++)
        {
            PrefabPool prefabPool = allPrefabPools[x];
            if (prefabPool == null)
            {
                allPrefabPools.RemoveAt(x);
                x -= 1;
            }
            else if (prefabPool.prefab == prefab)
            {
                return prefabPool;
            }
        }
        PrefabPool newPool = CreateInstance<PrefabPool>();
        newPool.parent = parent;
        newPool.prefab = prefab;
        allPrefabPools.Add(newPool);
        return newPool;
    }
}

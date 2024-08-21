using Ashen.ToolSystem;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolLookUp", menuName = "Custom/Managers/ToolLookUp")]
public class ToolLookUp : SingletonScriptableObject<ToolLookUp>
{
    private readonly Dictionary<GameObject, ToolManager> toolManagers = new Dictionary<GameObject, ToolManager>();

    public ToolManager this[GameObject go]
    {
        get
        {
            if (!go)
            {
                return null;
            }
            if (toolManagers.TryGetValue(go, out ToolManager tm))
            {
                return tm;
            }
            return null;
        }
    }

    public void Register(GameObject go, ToolManager tm)
    {
        if (go && tm)
        {
            if (this[go])
            {
                toolManagers[go] = tm;
            }
            else
            {
                toolManagers.Add(go, tm);
            }
        }
    }

    public void UnRegister(GameObject go)
    {
        if (this[go])
        {
            toolManagers.Remove(go);
        }
    }

    public ToolManager GetToolManager(GameObject go)
    {
        return this[go];
    }
}

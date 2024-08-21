using Ashen.ToolSystem;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttributeToolConfiguration", menuName = "Custom/Tool/BaseAttributeToolConfiguration")]
public class BaseAttributeToolConfiguration : A_Configuration<BaseAttributeTool, BaseAttributeToolConfiguration>
{
    [OdinSerialize]
    private Dictionary<BaseAttribute, int> defaultBase = default;

    public Dictionary<BaseAttribute, int> DefaultBase
    {
        get
        {
            if (defaultBase == null)
            {
                if (this == GetDefault())
                {
                    return null;
                }
                return GetDefault().DefaultBase;
            }
            else
            {
                Dictionary<BaseAttribute, int> derivedDefaultBase = new Dictionary<BaseAttribute, int>();
                foreach (BaseAttribute statAttribute in BaseAttributes.Instance)
                {
                    if (defaultBase.ContainsKey(statAttribute))
                    {
                        derivedDefaultBase.Add(statAttribute, defaultBase[statAttribute]);
                    }
                    else
                    {
                        if (this != GetDefault())
                        {
                            derivedDefaultBase.Add(statAttribute, GetDefault().defaultBase[statAttribute]);
                        }
                    }
                }
                return derivedDefaultBase;
            }
        }
    }
}

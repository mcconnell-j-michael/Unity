using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusToolConfiguration", menuName = "Custom/Tool/StatusToolConfiguration")]
public class StatusToolConfiguration : A_Configuration<StatusTool, StatusToolConfiguration>
{
    [OdinSerialize]
    private Dictionary<ResourceValue, DeliveryPackScriptableObject> statusEffectResultMap = default;
    [OdinSerialize]
    private List<StatusEffectScriptableObject> defaultStatusEffects = default;
    [OdinSerialize]
    private List<StatusEffectScriptableObject> additionalStatusEffects = default;

    public DeliveryPackScriptableObject[] StatusEffectResults
    {
        get
        {
            if (statusEffectResultMap == null)
            {
                if (this == GetDefault())
                {
                    return null;
                }
                return GetDefault().StatusEffectResults;
            }
            else
            {
                DeliveryPackScriptableObject[] derivedStatusEffectResults = new DeliveryPackScriptableObject[ResourceValues.Count];
                foreach (ResourceValue resourceValue in ResourceValues.Instance)
                {
                    if (statusEffectResultMap.ContainsKey(resourceValue))
                    {
                        derivedStatusEffectResults[(int)resourceValue] = statusEffectResultMap[resourceValue];
                    }
                    else
                    {
                        if (this != GetDefault())
                        {
                            derivedStatusEffectResults[(int)resourceValue] = GetDefault().StatusEffectResults[(int)resourceValue];
                        }
                    }
                }
                return derivedStatusEffectResults;
            }
        }
    }

    public List<StatusEffectScriptableObject> DefaultStatusEffects
    {
        get
        {
            List<StatusEffectScriptableObject> statusEffects = new List<StatusEffectScriptableObject>();
            if (defaultStatusEffects == null && this != GetDefault())
            {
                statusEffects = GetDefault().DefaultStatusEffects;
            }
            if (defaultStatusEffects != null)
            {
                statusEffects.AddRange(defaultStatusEffects);
            }
            if (additionalStatusEffects != null && this != GetDefault())
            {
                statusEffects.AddRange(additionalStatusEffects);
            }
            return statusEffects;
        }
    }

}

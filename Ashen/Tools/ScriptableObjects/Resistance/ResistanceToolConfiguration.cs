using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResistanceToolConfiguration", menuName = "Custom/Tool/ResistanceToolConfiguration")]
public class ResistanceToolConfiguration : A_Configuration<ResistanceTool, ResistanceToolConfiguration>
{
    [OdinSerialize]
    private Dictionary<DamageType, DerivedAttribute> resistanceEquations = default;
    [OdinSerialize]
    private AttributeLimiter resistanceAttributeLimiter = default;

    public Dictionary<DamageType, DerivedAttribute> ResistanceEquations
    {
        get
        {
            if (resistanceEquations == null)
            {
                if (this == GetDefault())
                {
                    return null;
                }
                return GetDefault().ResistanceEquations;
            }
            else
            {
                Dictionary<DamageType, DerivedAttribute> derivedResistanceEquations = new Dictionary<DamageType, DerivedAttribute>();
                foreach (DamageType damageType in DamageTypes.Instance)
                {
                    if (resistanceEquations.ContainsKey(damageType))
                    {
                        derivedResistanceEquations.Add(damageType, resistanceEquations[damageType]);
                    }
                    else
                    {
                        if (this != GetDefault())
                        {
                            derivedResistanceEquations.Add(damageType, GetDefault().resistanceEquations[damageType]);
                        }
                    }
                }
                return derivedResistanceEquations;
            }
        }
    }

    public AttributeLimiter ResistanceAttributeLimiter
    {
        get
        {
            if (resistanceAttributeLimiter == null)
            {
                if (this == GetDefault())
                {
                    return null;
                }
                return GetDefault().resistanceAttributeLimiter;
            }
            return resistanceAttributeLimiter;
        }
    }
}
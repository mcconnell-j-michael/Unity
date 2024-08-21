using Ashen.EquationSystem;
using Ashen.ToolSystem;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeToolConfiguration", menuName = "Custom/Tool/AttributeToolConfiguration")]
public class AttributeToolConfiguration : A_Configuration<AttributeTool, AttributeToolConfiguration>
{
    [OdinSerialize]
    private Dictionary<DerivedAttribute, Equation> overrideEquations = default;

    public Dictionary<DerivedAttribute, Equation> OverrideEquations
    {
        get
        {
            if (overrideEquations == null && this != GetDefault())
            {
                return GetDefault().OverrideEquations;
            }
            Dictionary<DerivedAttribute, Equation> overrides = new();
            foreach (DerivedAttribute attribute in DerivedAttributes.Instance)
            {
                if (overrideEquations.TryGetValue(attribute, out Equation eq))
                {
                    if (eq != null)
                    {
                        overrides.Add(attribute, eq);
                    }
                }
                else if (GetDefault().overrideEquations.TryGetValue(attribute, out eq))
                {
                    if (eq != null)
                    {
                        overrideEquations.Add(attribute, eq);
                    }
                }
            }
            return overrides;
        }
    }
}

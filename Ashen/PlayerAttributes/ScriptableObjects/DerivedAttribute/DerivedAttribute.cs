using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Ashen.ToolSystem;
using Sirenix.Serialization;
using System;
using UnityEngine;

/**
 * A character attribute defines the core values that make up the general power and effectiveness of a character
 **/
[CreateAssetMenu(fileName = nameof(DerivedAttribute), menuName = "Custom/Enums/" + nameof(DerivedAttributes) + "/Type")]
public class DerivedAttribute : A_Attribute<DerivedAttribute, DerivedAttributes, float>, I_EquationAttribute<AttributeTool, DerivedAttribute, float>
{
    public static string AttributeType = nameof(DerivedAttribute);
    [NonSerialized, OdinSerialize]
    public Equation equation;
    public bool percentage;

    public override float Get(I_DeliveryTool deliveryTool)
    {
        return Get(deliveryTool, null);
    }

    private float Get(I_DeliveryTool deliveryTool, DeliveryArgumentPacks equationArguments)
    {
        ToolManager toolManager = (deliveryTool as DeliveryTool).toolManager;
        if (toolManager)
        {
            AttributeTool at = toolManager.Get<AttributeTool>();
            if (at)
            {
                return at.Get(this, equationArguments);
            }
        }
        return 0;
    }

    public float GetAsFloat(float value)
    {
        return value;
    }

    public override string GetAttributeType()
    {
        return AttributeType;
    }
}

using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Ashen.ToolSystem;
using UnityEngine;

/**
 * A character attribute defines the core values that make up the general power and effectiveness of a character
 **/
[CreateAssetMenu(fileName = nameof(BaseAttribute), menuName = "Custom/Enums/" + nameof(BaseAttributes) + "/Type")]
public class BaseAttribute : A_Attribute<BaseAttribute, BaseAttributes, float>, I_EquationAttribute<BaseAttributeTool, BaseAttribute, float>
{
    public static string AttributeType = nameof(BaseAttribute);

    public override float Get(I_DeliveryTool deliveryTool)
    {
        ToolManager toolManager = (deliveryTool as DeliveryTool).toolManager;
        if (toolManager)
        {
            BaseAttributeTool at = toolManager.Get<BaseAttributeTool>();
            if (at)
            {
                return at.Get(this, null);
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

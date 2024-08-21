using Ashen.AbilitySystem;
using Ashen.DeliverySystem;

public class TargetAttribute : A_Attribute<TargetAttribute, TargetAttributes, Target>
{
    public static string AttributeType = nameof(TargetAttribute);

    public override Target Get(I_DeliveryTool deliveryTool)
    {
        return null;
    }

    public override string GetAttributeType()
    {
        return AttributeType;
    }
}

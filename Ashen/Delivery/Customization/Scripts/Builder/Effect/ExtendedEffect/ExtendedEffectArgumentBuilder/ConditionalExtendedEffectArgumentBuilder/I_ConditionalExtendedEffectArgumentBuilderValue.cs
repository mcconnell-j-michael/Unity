namespace Ashen.DeliverySystem
{
    public interface I_ConditionalExtendedEffectArgumentBuilderValue
    {
        int GetValue(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
    }
}

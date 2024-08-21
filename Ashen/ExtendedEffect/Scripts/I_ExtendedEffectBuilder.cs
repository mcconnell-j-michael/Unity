namespace Ashen.DeliverySystem
{
    public interface I_ExtendedEffectBuilder
    {
        ExtendedEffect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks);
    }
}
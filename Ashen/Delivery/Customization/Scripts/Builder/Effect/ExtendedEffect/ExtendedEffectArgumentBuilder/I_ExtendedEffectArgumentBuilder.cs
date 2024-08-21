namespace Ashen.DeliverySystem
{
    public interface I_ExtendedEffectArgumentBuilder
    {
        void FillArguments(ExtendedEffectArgumentFiller filler, I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
    }
}

namespace Ashen.DeliverySystem
{
    public interface I_ConditionalFilter
    {
        bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult);
    }
}
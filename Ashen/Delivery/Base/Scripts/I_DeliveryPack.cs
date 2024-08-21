namespace Ashen.DeliverySystem
{
    /**
     * This is the base interface that all delivery packs must used to be able to
     * plug into the delivery system
     **/
    public interface I_DeliveryPack
    {
        void FillArguments(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
        void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
    }
}

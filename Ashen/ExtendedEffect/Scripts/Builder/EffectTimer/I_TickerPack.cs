namespace Ashen.DeliverySystem
{
    /**
     * A TickerPack must know how to create an instance of its corresponding
     * Ticker
     **/
    public interface I_TickerPack
    {
        I_Ticker Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
    }
}

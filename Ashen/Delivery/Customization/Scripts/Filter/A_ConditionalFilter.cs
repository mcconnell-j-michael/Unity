using System;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public abstract class A_ConditionalFilter : I_ConditionalFilter
    {
        public abstract bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult);
    }
}
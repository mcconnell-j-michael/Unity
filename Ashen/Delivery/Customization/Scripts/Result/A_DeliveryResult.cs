using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * The delivery results contain information regarding what will be delivered to the target
     * if the DeliveryResult were to be published by its DeliveryResultsPack
     **/
    public abstract class A_DeliveryResult
    {
        public abstract void Clear();
        public abstract void Calculate(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
        public abstract void Deliver(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
        public abstract A_DeliveryResult Clone();
    }
}

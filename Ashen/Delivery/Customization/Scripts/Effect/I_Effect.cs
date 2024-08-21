
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * The I_Effect interface is what is required to be extended if a new effect type is created.
     **/
    public interface I_Effect
    {
        void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments);
    }
}

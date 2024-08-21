using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_DeliveryProcessor
    {
        void Process(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
    }
}
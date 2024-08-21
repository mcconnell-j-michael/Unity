using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_EffectCondition
    {
        bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
        string visualize();
    }
}
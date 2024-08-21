using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_ComponentBuilder
    {
        I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
    }
}
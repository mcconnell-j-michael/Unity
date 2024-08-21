using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_EffectBuilder
    {
        I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
        string visualize(int depth);
    }
}
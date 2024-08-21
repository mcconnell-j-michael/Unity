using UnityEngine;
using System.Collections;
using System;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class StatusEffectSymbolComponentBuilder : I_ComponentBuilder
    {
        public Sprite sprite;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new StatusEffectSymbolComponent(sprite);
        }
    }
}
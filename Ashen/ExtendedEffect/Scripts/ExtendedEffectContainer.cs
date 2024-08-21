using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public struct ExtendedEffectContainer
    {
        KeyContainer<I_ExtendedEffectComponent> container;
        public ExtendedEffectContainer(I_ExtendedEffectComponent statusEffect, string key, ExtendedEffectType type)
        {
            container = new KeyContainer<I_ExtendedEffectComponent>(statusEffect, key);
            this.type = type;
        }

        public ExtendedEffectContainer(I_ExtendedEffectComponent statusEffect, string key)
        {
            container = new KeyContainer<I_ExtendedEffectComponent>(statusEffect, key);
            this.type = ExtendedEffectType.NORMAL;
        }

        public ExtendedEffectType type;

        public I_ExtendedEffectComponent statusEffect
        {
            get
            {
                return container.source;
            }
        }

        public string key
        {
            get
            {
                return container.key;
            }
        }
    }
}
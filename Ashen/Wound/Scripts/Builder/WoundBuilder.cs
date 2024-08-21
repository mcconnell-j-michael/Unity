using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Ashen.WoundSystem
{
    [Serializable]
    public class WoundBuilder
    {
        [HideLabel, BoxGroup("Key")]
        public string key;

        [OdinSerialize]
        [ListDrawerSettings(ShowFoldout = false), InlineProperty, AutoPopulate]
        public List<I_ComponentBuilder> baseStatusEffects;

        public I_ExtendedEffect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks)
        {
            return new ExtendedEffect(baseStatusEffects, null, key, owner, target, deliveryArgumentPacks, false);
        }
    }
}
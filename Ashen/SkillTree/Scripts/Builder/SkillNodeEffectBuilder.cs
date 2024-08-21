using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    public class SkillNodeEffectBuilder
    {
        [OdinSerialize, HideLabel, BoxGroup("Key")]
        public string key;

        [ListDrawerSettings(ShowFoldout = false), InlineProperty, AutoPopulate]
        public List<I_ComponentBuilder> baseStatusEffects;

        public I_ExtendedEffect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks)
        {
            return new ExtendedEffect(baseStatusEffects, null, key, owner, target, deliveryArgumentPacks, false);
        }
    }
}
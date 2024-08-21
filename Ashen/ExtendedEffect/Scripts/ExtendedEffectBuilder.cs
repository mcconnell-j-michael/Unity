using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class ExtendedEffectBuilder : I_ExtendedEffectBuilder
    {
        [OdinSerialize, HideLabel, BoxGroup("Key")]
        public string key;

        [Hide, FoldoutGroup("Tag Options")]
        public TagHandler tagHandler;

        [ListDrawerSettings(ShowFoldout = false), InlineProperty, AutoPopulate]
        public List<I_ComponentBuilder> baseStatusEffects;

        public ExtendedEffect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks)
        {
            if (tagHandler != null)
            {
                StatusTool statusTool = ((DeliveryTool)target).toolManager.Get<StatusTool>();
                if (tagHandler != null)
                {
                    TagState state = tagHandler.Operate(owner, target, deliveryArgumentPacks);
                    if (!state.validStatusEffect)
                    {
                        return null;
                    }
                    if (state.appliedTags.Count > 0)
                    {
                        return new ExtendedEffect(baseStatusEffects, state.appliedTags, key, owner, target, deliveryArgumentPacks);
                    }
                }
            }
            return new ExtendedEffect(baseStatusEffects, null, key, owner, target, deliveryArgumentPacks);
        }
    }
}
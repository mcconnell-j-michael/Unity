using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    public class TargetingAbilityOverrideComponent : I_AbilityOverrideComponent
    {
        [HideLabel, EnumToggleButtons]
        public OverrideType type;

        [ShowIf(nameof(type), OverrideType.Override)]
        public TargetRange overrideRange;
        [ShowIf(nameof(type), OverrideType.Override)]
        public Target overrideTarget;
        [ShowIf(nameof(type), OverrideType.Override)]
        [ToggleGroup(nameof(shouldOverrideAbilityTags))]
        public bool shouldOverrideAbilityTags;
        [ShowIf(nameof(type), OverrideType.Override)]
        [ToggleGroup(nameof(shouldOverrideAbilityTags))]
        public List<AbilityTag> overrideAbilityTags;
        [EnumToggleButtons, Title("Who to target"), HideLabel]
        [ShowIf(nameof(type), OverrideType.Override)]
        public TargetParty targetParty;

        [Hide]
        [ShowIf(nameof(type), OverrideType.Replace)]
        public AbilityTargeting abilityTargeting;

        public void Override(AbilityAction abilityAction)
        {
            TargetingProcessor processor = abilityAction.Get<TargetingProcessor>();
            AbilityTargetingProcessor targetingProcessor = processor.GetTargetingValue() as AbilityTargetingProcessor;
            if (targetingProcessor == null)
            {
                return;
            }
            if (type == OverrideType.Override)
            {
                if (overrideTarget != null)
                {
                    targetingProcessor.customTarget = overrideTarget;
                }
                if (overrideRange != null)
                {
                    targetingProcessor.customRange = overrideRange;
                }
                if (shouldOverrideAbilityTags)
                {
                    targetingProcessor.CustomAbilityTags = overrideAbilityTags;
                }
                targetingProcessor.targetParty = targetParty;
            }
            else if (type == OverrideType.Replace)
            {
                AbilityTargeting targeting = abilityTargeting;
                if (targeting.targetType == TargetTypeInspector.Attribute)
                {
                    if (targeting.attribute.targetAttribute != null)
                    {
                        targetingProcessor.targetAttribute = targeting.attribute.targetAttribute;
                    }
                    if (targeting.attribute.overrideTarget != null)
                    {
                        targetingProcessor.customTarget = targeting.attribute.overrideTarget;
                    }
                    if (targeting.attribute.overrideRange != null)
                    {
                        targetingProcessor.customRange = targeting.attribute.overrideRange;
                    }
                    if (targeting.attribute.shouldOverrideAbilityTags)
                    {
                        targetingProcessor.CustomAbilityTags = targeting.attribute.overrideAbilityTags;
                    }
                }
                else if (targeting.targetType == TargetTypeInspector.Custom)
                {
                    targetingProcessor.customTarget = targeting.custom.target;
                    targetingProcessor.customRange = targeting.custom.range;
                    targetingProcessor.CustomAbilityTags = targeting.custom.abilityTags;
                }
                targetingProcessor.targetParty = targeting.targetParty;
            }
        }
    }
}
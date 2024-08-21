using Ashen.ItemSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityTargeting : I_AbilityBuilder, I_ItemBuilder
    {
        [TitleGroup("Target Type"), EnumToggleButtons, HideLabel]
        public TargetTypeInspector targetType;

        [Hide, ShowIf(nameof(targetType), Value = TargetTypeInspector.Custom)]
        public AbilityTargetingCustom custom;

        [Hide, ShowIf(nameof(targetType), Value = TargetTypeInspector.Attribute)]
        public AbilityTargetingAttribute attribute;

        [EnumToggleButtons, Title("Who to target"), HideLabel]
        public TargetParty targetParty;

        public I_AbilityProcessor Build(Ability ability)
        {
            AbilityTargetingProcessor processor = new AbilityTargetingProcessor();

            if (targetType == TargetTypeInspector.Attribute)
            {
                processor.targetAttribute = attribute.targetAttribute;
                if (attribute.overrideTarget != null)
                {
                    processor.customTarget = attribute.overrideTarget;
                }
                if (attribute.overrideRange != null)
                {
                    processor.customRange = attribute.overrideRange;
                }
                if (attribute.shouldOverrideAbilityTags)
                {
                    processor.CustomAbilityTags = attribute.overrideAbilityTags;
                }
                if (attribute.ruleOverrides != null)
                {
                    processor.customRules = new List<I_TargetingRule>();
                    processor.customRules.AddRange(attribute.ruleOverrides.GetRules());
                }
            }
            else if (targetType == TargetTypeInspector.Custom)
            {
                processor.customTarget = custom.target;
                processor.customRange = custom.range;
                processor.CustomAbilityTags = custom.abilityTags;
                processor.customRules = new List<I_TargetingRule>();
                if (custom.targetingRules == null)
                {
                    processor.customRules.AddRange(DefaultValues.Instance.config.GetConfiguration<ShiftableTargetingRuleToolConfiguration>().DefaultTargetingRule.GetRules());
                }
                else
                {
                    processor.customRules.AddRange(custom.targetingRules.GetRules());
                }
            }
            processor.targetParty = targetParty;

            return new TargetingProcessor(processor);
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new AbilityTargeting();
        }

        public string GetTabName()
        {
            return "Targeting";
        }
    }
}
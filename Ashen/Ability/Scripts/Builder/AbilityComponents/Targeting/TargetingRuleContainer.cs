using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class TargetingRuleContainer
    {
        [TitleGroup("Rule Type"), EnumToggleButtons, HideLabel]
        public TargetingRuleType type;

        [ShowIf(nameof(type), Value = TargetingRuleType.Pack)]
        public TargetingRulePack pack;

        [AutoPopulate, ShowIf(nameof(type), Value = TargetingRuleType.Custom)]
        public List<I_TargetingRule> rules;

        public List<I_TargetingRule> GetRules()
        {
            if (type == TargetingRuleType.Pack)
            {
                if (!pack)
                {
                    return new List<I_TargetingRule>();
                }
                return pack.targetingRules;
            }
            return rules;
        }
    }
}
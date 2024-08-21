using Ashen.AbilitySystem;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableTargetingRuleToolConfiguration : A_ShiftableAttributeToolConfiguration<ShiftableTargetingRuleTool, ShiftableTargetingRuleToolConfiguration, TargetAttribute, List<I_TargetingRule>>
    {
        [OdinSerialize]
        private TargetingRuleContainer defaultTargetingRule;

        public TargetingRuleContainer DefaultTargetingRule
        {
            get
            {
                if (defaultTargetingRule == null && this != GetDefault())
                {
                    return GetDefault().defaultTargetingRule;
                }
                return defaultTargetingRule;
            }
            private set { defaultTargetingRule = value; }
        }
    }
}
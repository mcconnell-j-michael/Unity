using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityTargetingCustom
    {
        public TargetRange range;
        public Target target;
        public List<AbilityTag> abilityTags;
        public TargetingRuleContainer targetingRules;
    }
}
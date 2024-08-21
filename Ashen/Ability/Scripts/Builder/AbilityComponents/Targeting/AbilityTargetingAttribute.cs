using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityTargetingAttribute
    {
        public TargetAttribute targetAttribute;
        [FoldoutGroup("Override Targeting")]
        public TargetRange overrideRange;
        [FoldoutGroup("Override Targeting")]
        public Target overrideTarget;
        [ToggleGroup("Override Targeting/" + nameof(shouldOverrideAbilityTags))]
        public bool shouldOverrideAbilityTags;
        [ToggleGroup("Override Targeting/" + nameof(shouldOverrideAbilityTags))]
        [ShowIf(nameof(shouldOverrideAbilityTags))]
        public List<AbilityTag> overrideAbilityTags;
        [FoldoutGroup("Override Targeting")]
        public TargetingRuleContainer ruleOverrides;
    }
}
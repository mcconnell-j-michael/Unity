using Sirenix.OdinInspector;
using System;

namespace Ashen.SkillTree
{
    [Serializable]
    public class ScaleAbility : ScaleDeliveryPack
    {
        [ToggleGroup(nameof(enableAbilityOverride))]
        public bool enableAbilityOverride;

        [Hide, ToggleGroup(nameof(enableAbilityOverride))]
        public AbilityOverrideContainer abilityOverrideContainer;
    }
}
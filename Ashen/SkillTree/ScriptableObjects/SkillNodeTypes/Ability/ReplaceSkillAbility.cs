using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.SkillTree
{
    [Serializable]
    public class ReplaceAbilitySkill
    {
        [HideLabel, EnumToggleButtons]
        public ReplaceAbilitySkillTypeInspector type;

        [ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject)]
        public AbilitySO ability;

        [ToggleGroup(nameof(enableAbilityOverride)), ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject)]
        public bool enableAbilityOverride;

        [Hide, ToggleGroup(nameof(enableAbilityOverride)), ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject)]
        public AbilityOverrideContainer abilityOverrideContainer;

        [Hide, ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.Custom)]
        public AbilityBuilder builder;
    }
}
using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    [Serializable]
    public class SubAbilitySkillNodeOverrideList
    {
        [EnumToggleButtons, HideLabel]
        public SubAbilityOptions option;

        [Hide, ShowIf(nameof(option), Value = SubAbilityOptions.ReplaceAll)]
        public List<SubAbilityBuilder> replacementSubAbilities;

        [Hide, ShowIf(nameof(option), Value = SubAbilityOptions.Individual)]
        public List<SubAbilitySkillNodeOverride> subAbilities;

        public enum SubAbilityOptions
        {
            Individual, ReplaceAll
        }
    }
}
using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.SkillTree
{
    [Serializable]
    public class SubAbilitySkillNodeOverride
    {
        [EnumToggleButtons]
        public SkillNodeOverrideOptions options;

        [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.Scale)]
        public ScaleSubAbility scaleDeliveryPack;

        [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.New)]
        public SubAbilityBuilder replaceSkillAbility;
    }
}
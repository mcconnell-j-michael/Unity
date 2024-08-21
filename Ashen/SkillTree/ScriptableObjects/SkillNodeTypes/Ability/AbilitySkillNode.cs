using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.SkillTree
{
    [Serializable]
    public class AbilitySkillNode : A_SkillNode<AbilitySkillNodeOverride>
    {
        [ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject), PropertyOrder(-1)]
        public AbilitySO baseAbility;

        [Hide, ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.Custom), PropertyOrder(-1)]
        public AbilityBuilder builder;
    }
}
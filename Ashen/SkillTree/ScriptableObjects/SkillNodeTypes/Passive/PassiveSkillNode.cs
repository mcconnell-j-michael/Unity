using Sirenix.OdinInspector;
using System;

namespace Ashen.SkillTree
{
    [Serializable]
    public class PassiveSkillNode : A_SkillNode<PassiveSkillNodeOverride>
    {
        [ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.ScriptableObject), PropertyOrder(-1)]
        public PassiveScriptableObject baseAbility;

        [ShowIf(nameof(type), Value = ReplaceAbilitySkillTypeInspector.Custom), PropertyOrder(-1)]
        [Hide]
        public SkillNodeEffectBuilder builder;
    }
}
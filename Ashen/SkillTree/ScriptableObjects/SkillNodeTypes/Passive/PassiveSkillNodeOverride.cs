using Sirenix.OdinInspector;
using System;

namespace Ashen.SkillTree
{
    [Serializable]
    public class PassiveSkillNodeOverride
    {
        [EnumToggleButtons]
        public SkillNodeOverrideOptions options;

        [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.Scale)]
        public ScaleDeliveryPack scaleDeliveryPack;

        [Hide, ShowIf(nameof(options), Value = SkillNodeOverrideOptions.New)]
        public ReplacePassiveSkill replaceSkillAbility;
    }
}
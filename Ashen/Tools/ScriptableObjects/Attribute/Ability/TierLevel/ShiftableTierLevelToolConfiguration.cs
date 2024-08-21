using Sirenix.OdinInspector;
using UnityEngine;

namespace Ashen.ToolSystem
{
    public class ShiftableTierLevelToolConfiguration : A_Configuration<ShiftableTierLevelTool, ShiftableTierLevelToolConfiguration>
    {
        [SerializeField, HideIf("@" + nameof(IsDefault) + "()")]
        private bool overrideAbilityTierLevelLimit;
        [SerializeField, ShowIf("@" + nameof(ShowLimit) + "()")]
        private DerivedAttribute abilityTierLevelLimit;
        private bool ShowLimit() { return IsDefault() || overrideAbilityTierLevelLimit; }

        public DerivedAttribute AbilityTierLevelLimit
        {
            get
            {
                if (IsDefault())
                {
                    return abilityTierLevelLimit;
                }
                if (overrideAbilityTierLevelLimit)
                {
                    return abilityTierLevelLimit;
                }
                return GetDefault().AbilityTierLevelLimit;
            }
        }
    }
}
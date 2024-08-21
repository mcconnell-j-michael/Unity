using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public class AbilityTag : A_EnumSO<AbilityTag, AbilityTags>, I_EquationAttribute<ShiftableTierLevelTool, AbilityTag, int>
    {
        public ExtendedEffectTrigger effectTrigger;

        public float GetAsFloat(int value)
        {
            return value;
        }
    }
}
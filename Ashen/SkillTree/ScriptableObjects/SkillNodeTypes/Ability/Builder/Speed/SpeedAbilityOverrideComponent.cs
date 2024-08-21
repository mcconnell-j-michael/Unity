using Ashen.AbilitySystem;
using Ashen.EquationSystem;
using Ashen.VariableSystem;
using Sirenix.OdinInspector;
using static Ashen.AbilitySystem.AbilitySpeed;

namespace Ashen.SkillTree
{
    public class SpeedAbilityOverrideComponent : I_AbilityOverrideComponent
    {
        [EnumToggleButtons, HideLabel]
        public SpeedOptionInspector option;

        [ShowIf(nameof(option), Value = SpeedOptionInspector.Category)]
        public AbilitySpeedCategory speedCategory;
        [ShowIf("@" + nameof(speedCategory) + " == null || " +
            nameof(speedCategory) + "." + nameof(AbilitySpeedCategory.useSpeedCalculation) + " || " +
            nameof(option) + " == " + nameof(SpeedOptionInspector) + "." + nameof(SpeedOptionInspector.SpeedFactor))]
        public Reference<I_Equation> speedEquation;

        public void Override(AbilityAction abilityAction)
        {
            SpeedProcessor speedProcessor = abilityAction.Get<SpeedProcessor>();
            if (speedCategory)
            {
                speedProcessor.SetSpeedCategory(speedCategory);
            }
            if (speedEquation != null && speedEquation.Value != null)
            {
                speedProcessor.SetSpeedFactor(speedEquation.Value);
            }
        }
    }
}
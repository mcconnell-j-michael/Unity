using Ashen.AbilitySystem;
using Sirenix.OdinInspector;

namespace Ashen.SkillTree
{
    public class SpeedSubAbilityOverrideComponent : I_SubAbilityOverrideComponent
    {
        [EnumToggleButtons]
        public RelativeSpeed relativeSpeed;
        [ShowIf(nameof(relativeSpeed), Value = RelativeSpeed.Unique)]
        public AbilitySpeed speed;

        public void Override(AbilityAction abilityAction)
        {
            SpeedProcessor speedProcessor = abilityAction.Get<SpeedProcessor>();
            speedProcessor.SetSpeedFactor(speed.speedEquation.Value);

            SubAbilitySpeedValue subSpeedProcessor = speedProcessor.GetBaseSpeedProcessor() as SubAbilitySpeedValue;
            if (subSpeedProcessor != null)
            {
                subSpeedProcessor.SetRelativeSpeed(relativeSpeed);
            }
        }
    }
}
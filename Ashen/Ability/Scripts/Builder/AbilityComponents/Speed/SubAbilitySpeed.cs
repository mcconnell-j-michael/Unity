using Sirenix.OdinInspector;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class SubAbilitySpeed : I_SubAbilityBuilder
    {
        [EnumToggleButtons]
        public RelativeSpeed relativeSpeed;
        [ShowIf(nameof(relativeSpeed), Value = RelativeSpeed.Unique)]
        public AbilitySpeed speed;

        public I_AbilityProcessor Build(Ability ability)
        {
            SubAbilitySpeedValue processor = new SubAbilitySpeedValue();
            SpeedProcessor parentProcessor = ability.abilityAction.Get<SpeedProcessor>();
            processor.parentProcessor = parentProcessor;

            if (relativeSpeed == RelativeSpeed.Unique)
            {
                if (speed.option == AbilitySpeed.SpeedOptionInspector.SpeedFactor)
                {
                    processor.speedFactor = speed.speedEquation.Value;
                    processor.speedCategory = AbilitySpeedCategories.Instance.defaultSpeedCategory;
                }
                else if (speed.option == AbilitySpeed.SpeedOptionInspector.Category)
                {
                    processor.speedCategory = speed.speedCategory;
                    if (speed.speedCategory.useSpeedCalculation)
                    {
                        processor.speedFactor = null;
                    }
                }
            }
            processor.relativeSpeed = relativeSpeed;

            return new SpeedProcessor(processor);
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new SubAbilitySpeed();
        }

        public string GetTabName()
        {
            return "Speed";
        }
    }
}
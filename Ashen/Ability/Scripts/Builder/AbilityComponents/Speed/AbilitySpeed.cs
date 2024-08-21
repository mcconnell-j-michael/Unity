using Ashen.EquationSystem;
using Ashen.ItemSystem;
using Ashen.VariableSystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilitySpeed : I_AbilityBuilder, I_ItemBuilder
    {
        [EnumToggleButtons, HideLabel]
        public SpeedOptionInspector option;

        [ShowIf(nameof(option), Value = SpeedOptionInspector.Category)]
        public AbilitySpeedCategory speedCategory;
        [ShowIf("@" + nameof(speedCategory) + " == null || " +
            nameof(speedCategory) + "." + nameof(AbilitySpeedCategory.useSpeedCalculation) + " || " +
            nameof(option) + " == " + nameof(SpeedOptionInspector) + "." + nameof(SpeedOptionInspector.SpeedFactor))]
        public Reference<I_Equation> speedEquation;

        public I_AbilityProcessor Build(Ability ability)
        {
            AbilitySpeedValue processor = new AbilitySpeedValue();
            if (option == AbilitySpeed.SpeedOptionInspector.SpeedFactor)
            {
                processor.speedFactor = speedEquation.Value;
                processor.speedCategory = AbilitySpeedCategories.Instance.defaultSpeedCategory;
            }
            else if (option == AbilitySpeed.SpeedOptionInspector.Category)
            {
                processor.speedCategory = speedCategory;
                if (speedCategory.useSpeedCalculation)
                {
                    processor.speedFactor = null;
                }
            }
            return new SpeedProcessor(processor);
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new AbilitySpeed();
        }

        public string GetTabName()
        {
            return "Speed";
        }

        public enum SpeedOptionInspector
        {
            SpeedFactor, Category
        }
    }
}
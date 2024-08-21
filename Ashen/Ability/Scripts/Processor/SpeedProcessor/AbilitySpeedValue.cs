using Ashen.EquationSystem;

namespace Ashen.AbilitySystem
{
    public class AbilitySpeedValue : I_BaseAbilitySpeedProcessor
    {
        public AbilitySpeedCategory speedCategory;
        public I_Equation speedFactor;

        public I_Equation GetSpeedFactor()
        {
            return speedFactor;
        }

        public AbilitySpeedCategory GetSpeedCategory()
        {
            return speedCategory;
        }

        public void SetSpeedFactor(I_Equation speedFactor)
        {
            this.speedFactor = speedFactor;
        }

        public void SetSpeedCategory(AbilitySpeedCategory category)
        {
            speedCategory = category;
        }

        public void SetRelativeSpeed(RelativeSpeed relativeSpeed)
        {
        }

        public RelativeSpeed GetRelativeSpeed()
        {
            return default;
        }
    }
}
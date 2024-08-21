using Ashen.EquationSystem;

namespace Ashen.AbilitySystem
{
    public class SubAbilitySpeedValue : I_BaseAbilitySpeedProcessor
    {
        public SpeedProcessor parentProcessor;

        public AbilitySpeedCategory speedCategory;
        public RelativeSpeed relativeSpeed;
        public I_Equation speedFactor;

        public I_Equation GetSpeedFactor()
        {
            return speedFactor;
        }

        public AbilitySpeedCategory GetSpeedCategory()
        {
            switch (relativeSpeed)
            {
                case RelativeSpeed.After:
                case RelativeSpeed.Before:
                    return parentProcessor.GetSpeedCategory();
                case RelativeSpeed.Unique:
                    return speedCategory;
                default:
                    if (speedCategory == null)
                    {
                        return parentProcessor.GetSpeedCategory();
                    }
                    return speedCategory;
            }
        }

        public void SetSpeedFactor(I_Equation speedFactor)
        {
            this.speedFactor = speedFactor;
        }

        public void SetSpeedCategory(AbilitySpeedCategory category)
        {
            this.speedCategory = category;
        }

        public void SetRelativeSpeed(RelativeSpeed relativeSpeed)
        {
            this.relativeSpeed = relativeSpeed;
        }

        public RelativeSpeed GetRelativeSpeed()
        {
            return relativeSpeed;
        }
    }
}
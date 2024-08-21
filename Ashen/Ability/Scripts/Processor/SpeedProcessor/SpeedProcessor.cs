using Ashen.EquationSystem;

namespace Ashen.AbilitySystem
{
    public class SpeedProcessor : A_AbilityProcessor
    {
        private I_BaseAbilitySpeedProcessor speedProcessor;

        public SpeedProcessor(I_BaseAbilitySpeedProcessor speedProcessor)
        {
            this.speedProcessor = speedProcessor;
        }

        public I_Equation GetSpeedFactor()
        {
            return speedProcessor.GetSpeedFactor();
        }

        public AbilitySpeedCategory GetSpeedCategory()
        {
            return speedProcessor.GetSpeedCategory();
        }

        public I_BaseAbilitySpeedProcessor GetBaseSpeedProcessor()
        {
            return speedProcessor;
        }

        public void SetSpeedFactor(I_Equation speedFactor)
        {
            speedProcessor.SetSpeedFactor(speedFactor);
        }

        public void SetSpeedCategory(AbilitySpeedCategory category)
        {
            speedProcessor.SetSpeedCategory(category);
        }
    }
}

using Ashen.EquationSystem;

namespace Ashen.AbilitySystem
{
    public interface I_BaseAbilitySpeedProcessor
    {
        I_Equation GetSpeedFactor();
        AbilitySpeedCategory GetSpeedCategory();
        void SetSpeedFactor(I_Equation speedFactor);
        void SetSpeedCategory(AbilitySpeedCategory category);
    }
}

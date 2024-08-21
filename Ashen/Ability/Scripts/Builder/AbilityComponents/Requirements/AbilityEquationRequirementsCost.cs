using Sirenix.OdinInspector;

namespace Ashen.AbilitySystem
{
    public class AbilityEquationRequirementsCost
    {
        [AutoPopulate, HideWithoutAutoPopulate, Title("Left Side"), PropertyOrder(0f)]
        public AbilityRequirementsCostIndividual leftSide;
        [AutoPopulate, HideWithoutAutoPopulate, Title("Right Side"), PropertyOrder(1.5f)]
        public AbilityRequirementsCostIndividual rightSide;
    }
}

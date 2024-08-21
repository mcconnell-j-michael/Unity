using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Ashen.AbilitySystem
{
    public class DeliveryValueRequirement : I_AbilityRequirement
    {
        [HorizontalGroup(nameof(DeliveryValueRequirement))]
        [VerticalGroup(nameof(DeliveryValueRequirement) + "/" + nameof(left))]
        [HideLabel, Title(nameof(left)), OdinSerialize]
        private I_DeliveryValue left;

        [VerticalGroup(nameof(DeliveryValueRequirement) + "/" + nameof(comparison))]
        [HideLabel, Title(nameof(comparison)), EnumToggleButtons, OdinSerialize]
        private RequirementComparison comparison = RequirementComparison.GT;

        [VerticalGroup(nameof(DeliveryValueRequirement) + "/" + nameof(right))]
        [HideLabel, Title(nameof(right)), OdinSerialize]
        private I_DeliveryValue right;

        public bool IsValid(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool dTool = toolManager.Get<DeliveryTool>();
            float leftSide = left.Build(dTool, dTool, deliveryArguments);
            float rightSide = right.Build(dTool, dTool, deliveryArguments);
            switch (comparison)
            {
                case RequirementComparison.GT:
                    return leftSide > rightSide;
                case RequirementComparison.LT:
                    return leftSide < rightSide;
                case RequirementComparison.GT_EQ:
                    return leftSide >= rightSide;
                case RequirementComparison.LT_EQ:
                    return leftSide <= rightSide;
                case RequirementComparison.EQ:
                    return leftSide == rightSide;
                case RequirementComparison.NOT_EQ:
                    return leftSide != rightSide;
            }
            return false;
        }
    }
}

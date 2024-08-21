using Ashen.EquationSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class ResourceAbilityRequirement : I_AbilityRequirement
    {
        [EnumToggleButtons, HideLabel]
        public CostTypeInspector costType;

        [HideLabel, Title(nameof(comparison)), EnumToggleButtons]
        public RequirementComparison comparison = RequirementComparison.GT;

        [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
        [ShowIf(nameof(costType), CostTypeInspector.ActionPoint)]
        public I_Equation actionPointValue;

        [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
        [ShowIf(nameof(costType), CostTypeInspector.PrimaryResource)]
        public I_Equation primaryResourceValue;

        [AutoPopulate(typeof(Equation)), HideWithoutAutoPopulate, Title("Value")]
        [ShowIf(nameof(costType), CostTypeInspector.Health)]
        public I_Equation healthValue;

        [ShowIf(nameof(costType), CostTypeInspector.Custom), Hide]
        public AbilityRequirementsCostCustom customRequirements;

        [ShowIf(nameof(costType), CostTypeInspector.Equation), Hide]
        public AbilityEquationRequirementsCost equation;

        public bool IsValid(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments)
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            DeliveryTool dTool = toolManager.Get<DeliveryTool>();
            int rightSide = 0;
            int leftSide = 0;
            switch (costType)
            {
                case CostTypeInspector.ActionPoint:
                    leftSide = rvTool.GetValue(ResourceValues.Instance.ACTION_POINT).currentValue;
                    rightSide = (int)primaryResourceValue.Calculate(dTool);
                    break;
                case CostTypeInspector.PrimaryResource:
                    leftSide = rvTool.GetValue(ResourceValues.Instance.ABILITY_RESOURCE).currentValue;
                    rightSide = (int)primaryResourceValue.Calculate(dTool);
                    break;
                case CostTypeInspector.Health:
                    leftSide = rvTool.GetValue(ResourceValues.Instance.health).currentValue;
                    rightSide = (int)healthValue.Calculate(dTool);
                    break;
                case CostTypeInspector.Custom:
                    leftSide = rvTool.GetValue(customRequirements.resourceValue).currentValue;
                    rightSide = customRequirements.GetValue(dTool, deliveryArguments);
                    break;
                case CostTypeInspector.Equation:
                    leftSide = (int)equation.leftSide.GetValue(dTool, deliveryArguments);
                    rightSide = (int)equation.rightSide.GetValue(dTool, deliveryArguments);
                    break;
            }
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
            return leftSide <= rightSide;
        }
    }
}
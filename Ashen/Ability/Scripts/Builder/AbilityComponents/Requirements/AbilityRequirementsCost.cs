using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityRequirementsCost
    {
        [ToggleGroup(nameof(enableAP), "Action Points")]
        public bool enableAP;
        [ToggleGroup(nameof(enableAP))]
        [AutoPopulate, HideWithoutAutoPopulate, Title("Action Points")]
        public AbilityRequirementsCostIndividual actionPoints;
        [ToggleGroup(nameof(enableRV), "Resource Value")]
        public bool enableRV;
        [ToggleGroup(nameof(enableRV))]
        [AutoPopulate, HideWithoutAutoPopulate, Title("Resource Value")]
        public AbilityRequirementsCostIndividual resourceValue;
        [ToggleGroup(nameof(enableHP), "Health")]
        public bool enableHP;
        [ToggleGroup(nameof(enableHP))]
        [AutoPopulate, HideWithoutAutoPopulate, Title("Health")]
        public AbilityRequirementsCostIndividual health;

        [AutoPopulate]
        public List<AbilityRequirementsCostCustom> customRequirements;

        public Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> GetCosts()
        {
            Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> values = new();
            if (enableAP)
            {
                values.Add(ResourceValues.Instance.ACTION_POINT, new() { actionPoints });
            }
            if (enableRV)
            {
                values.Add(ResourceValues.Instance.ABILITY_RESOURCE, new() { resourceValue });
            }
            if (enableHP)
            {
                values.Add(ResourceValues.Instance.health, new() { health });
            }
            if (customRequirements == null)
            {
                return values;
            }
            foreach (AbilityRequirementsCostCustom custom in customRequirements)
            {
                if (custom.customResourceValue != null
                    && custom.resourceValue
                    && values.TryGetValue(custom.resourceValue, out List<AbilityRequirementsCostIndividual> costs))
                {
                    costs.Add(custom.customResourceValue);
                }
            }
            return values;
        }
    }

    public enum CostTypeInspector
    {
        ActionPoint, PrimaryResource, Health, Custom, Equation
    }

    public enum ActionPointCostType
    {
        ActionPointValueRange, ActionPointEquation
    }

    public enum PrimaryResourceCostType
    {
        RVValueRange, RVEquation
    }
}
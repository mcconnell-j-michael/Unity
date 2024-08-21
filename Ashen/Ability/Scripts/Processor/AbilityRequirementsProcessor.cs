using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class AbilityRequirementsProcessor : A_AbilityProcessor
    {
        [ShowInInspector, ReadOnly]
        private List<I_AbilityRequirement> requirements;
        public List<I_AbilityRequirement> Requirements
        {
            get
            {
                return requirements;
            }
            set
            {
                if (value == null)
                {
                    requirements = null;
                }
                else
                {
                    requirements = new List<I_AbilityRequirement>();
                    requirements.AddRange(value);
                }
            }
        }

        [ShowInInspector, ReadOnly]
        private List<I_DeliveryValue>[] resourceCosts;

        [ShowInInspector, ReadOnly]
        private List<I_DeliveryValue>[] resourceGenerators;

        public void ResetResourceCosts(ResourceValue value)
        {
            if (resourceCosts != null && resourceCosts[(int)value] != null)
            {
                resourceCosts[(int)value].Clear();
            }
        }

        public void ResetResourceGenerators(ResourceValue value)
        {
            if (resourceGenerators != null && resourceGenerators[(int)value] != null)
            {
                resourceGenerators[(int)value].Clear();
            }
        }

        public void SetResourceCost(ResourceValue resourceValue, I_DeliveryValue equation)
        {
            if (resourceCosts == null)
            {
                resourceCosts = new List<I_DeliveryValue>[ResourceValues.Count];
            }
            if (resourceCosts[(int)resourceValue] == null)
            {
                resourceCosts[(int)resourceValue] = new List<I_DeliveryValue>();
            }
            resourceCosts[(int)resourceValue].Add(equation);
        }

        public void SetResourceGeneration(ResourceValue resourceValue, I_DeliveryValue equation)
        {
            if (resourceGenerators == null)
            {
                resourceGenerators = new List<I_DeliveryValue>[ResourceValues.Count];
            }
            if (resourceGenerators[(int)resourceValue] == null)
            {
                resourceGenerators[(int)resourceValue] = new List<I_DeliveryValue>();
            }
            resourceGenerators[(int)resourceValue].Add(equation);
        }

        public int GetResourceChange(ResourceValue resourceValue, ToolManager toolManager, DeliveryArgumentPacks arguments, ResourceChangeType resourceChangeType = ResourceChangeType.DIFFERENCE)
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            DeliveryTool dTool = toolManager.Get<DeliveryTool>();
            float cost = 0;
            float generate = 0;
            int index = (int)resourceValue;

            if (resourceCosts != null && resourceCosts[index] != null)
            {
                foreach (I_DeliveryValue deliveryValue in resourceCosts[index])
                {
                    cost += deliveryValue.Build(dTool, dTool, arguments);
                }
            }
            if (resourceGenerators != null && resourceGenerators[index] != null)
            {
                foreach (I_DeliveryValue deliveryValue in resourceGenerators[index])
                {
                    generate += deliveryValue.Build(dTool, dTool, arguments);
                }
            }

            return resourceChangeType switch
            {
                ResourceChangeType.COST => (int)cost,
                ResourceChangeType.GENERATE => (int)generate,
                ResourceChangeType.DIFFERENCE => (int)(cost - generate),
                _ => (int)(cost - generate),
            };
        }

        public bool IsValid(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments)
        {
            if (requirements == null || requirements.Count == 0)
            {
                return true;
            }
            foreach (I_AbilityRequirement requirement in requirements)
            {
                if (!requirement.IsValid(toolManager, deliveryArguments))
                {
                    return false;
                }
            }
            return true;
        }

        public override void OnExecute(ToolManager toolManager, DeliveryArgumentPacks arguments)
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            foreach (ResourceValue rv in ResourceValues.Instance)
            {
                int change = GetResourceChange(rv, toolManager, arguments);
                if (change < 0)
                {
                    rvTool.RemoveAmount(rv, -change);
                }
                else if (change > 0)
                {
                    rvTool.ApplyAmount(rv, change);
                }
            }
        }
    }

    public enum ResourceChangeType
    {
        COST, GENERATE, DIFFERENCE
    }
}
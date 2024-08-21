using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityRequirements : I_AbilityBuilder
    {
        [FoldoutGroup("Requirements"), AutoPopulate, ListDrawerSettings(ShowFoldout = false)]
        public List<I_AbilityRequirement> requirements;

        [ToggleGroup(nameof(hasCosts))]
        public bool hasCosts;
        [ToggleGroup(nameof(hasCosts)), Hide]
        public AbilityRequirementsCost costs;

        [ToggleGroup(nameof(doesGenerate))]
        public bool doesGenerate;
        [ToggleGroup(nameof(doesGenerate)), Hide]
        public AbilityRequirementsCost generates;

        public I_AbilityProcessor Build(Ability ability)
        {
            AbilityRequirementsProcessor processor = new AbilityRequirementsProcessor();

            if (requirements != null && requirements.Count > 0)
            {
                processor.Requirements = requirements;
            }
            if (hasCosts)
            {
                AbilityRequirementsCost cost = costs;
                Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> values = cost.GetCosts();
                HandleResources(values, (rv, svBuilder) => processor.SetResourceCost(rv, svBuilder));
            }
            if (doesGenerate)
            {
                AbilityRequirementsCost generate = generates;
                Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> values = generates.GetCosts();
                HandleResources(values, (rv, svBuilder) => processor.SetResourceGeneration(rv, svBuilder));
            }

            return processor;
        }

        private void HandleResources(
            Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> values,
            Action<ResourceValue, I_DeliveryValue> setValue
        )
        {
            foreach (KeyValuePair<ResourceValue, List<AbilityRequirementsCostIndividual>> pair in values)
            {
                if (pair.Value == null)
                {
                    continue;
                }
                foreach (AbilityRequirementsCostIndividual individual in pair.Value)
                {
                    if (individual.DeliveryValue == null)
                    {
                        continue;
                    }
                    setValue(pair.Key, individual.DeliveryValue);
                }
            }
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new AbilityRequirements();
        }

        public string GetTabName()
        {
            return "Requirements";
        }
    }


    public enum AbilityCostInspector
    {
        Use, Generate
    }
}
using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    public class RequirementsAbilityOverrideComponent : I_AbilityOverrideComponent
    {
        [ToggleGroup(nameof(shouldOverrideRequirements))]
        public bool shouldOverrideRequirements;
        [ToggleGroup(nameof(shouldOverrideRequirements)), EnumToggleButtons, HideLabel]
        public OverrideType overrideType;
        [ToggleGroup(nameof(shouldOverrideRequirements)), Hide, ListDrawerSettings(ShowFoldout = false), ShowIf(nameof(overrideType), OverrideType.Replace)]
        public List<I_AbilityRequirement> requirements;
        [ToggleGroup(nameof(shouldOverrideRequirements)), Hide, ListDrawerSettings(ShowFoldout = false, AlwaysAddDefaultValue = true), ShowIf(nameof(overrideType), OverrideType.Override)]
        public List<AbilityRequirementsType> overrideRequirements;

        [ToggleGroup(nameof(shouldOverrideCosts))]
        public bool shouldOverrideCosts;
        [ToggleGroup(nameof(shouldOverrideCosts)), Hide]
        public AbilityRequirementsCost costs;

        [ToggleGroup(nameof(shouldOverrideGenerators))]
        public bool shouldOverrideGenerators;
        [ToggleGroup(nameof(shouldOverrideGenerators)), Hide]
        public AbilityRequirementsCost generates;

        public void Override(AbilityAction abilityAction)
        {
            AbilityRequirementsProcessor requirementsProcessor = abilityAction.Get<AbilityRequirementsProcessor>();
            if (shouldOverrideRequirements)
            {
                if (overrideType == OverrideType.Override)
                {
                    if (requirementsProcessor.Requirements == null)
                    {
                        List<I_AbilityRequirement> requirements = new List<I_AbilityRequirement>();
                        foreach (AbilityRequirementsType type in overrideRequirements)
                        {
                            if (type.type == AbilityRequirementsTypeInspector.Override)
                            {
                                requirements.Add(type.requirement);
                            }
                        }
                        requirementsProcessor.Requirements = requirements;
                    }
                    else
                    {
                        int x = 0;
                        int y = 0;
                        List<AbilityRequirementsType> newRequirements = overrideRequirements;
                        if (newRequirements != null)
                        {
                            for (; x < requirementsProcessor.Requirements.Count && y < newRequirements.Count; y++)
                            {
                                AbilityRequirementsType newRequirement = newRequirements[y];
                                if (newRequirement.type == AbilityRequirementsTypeInspector.Override)
                                {
                                    requirementsProcessor.Requirements[x] = newRequirement.requirement;
                                    x++;
                                }
                                else if (newRequirement.type == AbilityRequirementsTypeInspector.Remove)
                                {
                                    requirementsProcessor.Requirements.RemoveAt(x);
                                }
                            }
                            for (; y < newRequirements.Count; x++)
                            {
                                AbilityRequirementsType newRequirement = newRequirements[y];
                                if (newRequirement.type == AbilityRequirementsTypeInspector.Override)
                                {
                                    requirementsProcessor.Requirements.Add(newRequirement.requirement);
                                }
                            }
                        }
                    }
                }
                else if (overrideType == OverrideType.Replace)
                {
                    requirementsProcessor.Requirements = requirements;
                }
            }
            if (shouldOverrideCosts)
            {
                AbilityRequirementsCost cost = costs;
                Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> values = cost.GetCosts();
                HandleResources(values, (rv, valueBuilders) =>
                    {
                        requirementsProcessor.ResetResourceCosts(rv);
                        foreach (I_DeliveryValue builder in valueBuilders)
                        {
                            requirementsProcessor.SetResourceCost(rv, builder);
                        }
                    }
                );
            }
            if (shouldOverrideGenerators)
            {
                AbilityRequirementsCost generate = generates;
                Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> values = generates.GetCosts();
                HandleResources(values, (rv, valueBuilders) =>
                    {
                        requirementsProcessor.ResetResourceGenerators(rv);
                        foreach (I_DeliveryValue builder in valueBuilders)
                        {
                            requirementsProcessor.SetResourceGeneration(rv, builder);
                        }
                    }
                );
            }
        }

        private void HandleResources(
            Dictionary<ResourceValue, List<AbilityRequirementsCostIndividual>> values,
            Action<ResourceValue, List<I_DeliveryValue>> setValue
        )
        {
            foreach (ResourceValue rv in ResourceValues.Instance)
            {
                List<I_DeliveryValue> equations = new();
                if (values.TryGetValue(rv, out List<AbilityRequirementsCostIndividual> individuals))
                {
                    if (individuals == null)
                    {
                        continue;
                    }
                    foreach (AbilityRequirementsCostIndividual individual in individuals)
                    {
                        if (individual.DeliveryValue == null)
                        {
                            continue;
                        }
                        equations.Add(individual.DeliveryValue);
                    }
                }
                if (equations.Count != 0)
                {
                    setValue(rv, equations);
                }
            }
        }
    }
}
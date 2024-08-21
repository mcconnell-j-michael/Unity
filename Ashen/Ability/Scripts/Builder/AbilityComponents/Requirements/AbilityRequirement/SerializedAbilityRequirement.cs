using Ashen.ToolSystem;
using System;
using UnityEngine;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class SerializedAbilityRequirement : I_AbilityRequirement
    {
        [SerializeField]
        private AbilityRequirementContainer container;

        public bool IsValid(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments)
        {
            if (container == null || container.abilityRequirements == null)
            {
                return true;
            }
            foreach (I_AbilityRequirement abilityRequirement in container.abilityRequirements)
            {
                if (!abilityRequirement.IsValid(toolManager, deliveryArguments))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
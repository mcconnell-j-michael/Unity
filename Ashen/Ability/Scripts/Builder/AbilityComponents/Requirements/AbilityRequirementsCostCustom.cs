using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityRequirementsCostCustom
    {
        [EnumSODropdown, HideLabel, Title("Resource Type")]
        public ResourceValue resourceValue;
        [AutoPopulate, HideWithoutAutoPopulate, Title("Value")]
        public AbilityRequirementsCostIndividual customResourceValue;

        public int GetValue(DeliveryTool dTool, DeliveryArgumentPacks deliveryArguments)
        {
            return customResourceValue.GetValue(dTool, deliveryArguments);
        }
    }
}
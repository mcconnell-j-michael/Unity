using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityRequirementsCostIndividual
    {
        [Title("Value"), OdinSerialize, HideLabel]
        private I_DeliveryValue deliveryValue;
        public I_DeliveryValue DeliveryValue { get { return deliveryValue; } }

        public int GetValue(DeliveryTool dTool, DeliveryArgumentPacks deliveryArguments)
        {
            return (int)deliveryValue.Build(dTool, dTool, deliveryArguments);
        }
    }
    public enum CostType
    {
        ValueRange, Equation
    }
}
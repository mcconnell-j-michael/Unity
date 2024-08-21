using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ConditionalExtendedEffectArgumentBuilder : I_ExtendedEffectArgumentBuilder
    {
        [OdinSerialize]
        private I_ConditionalExtendedEffectArgumentBuilderValue conditionalValue;
        [OdinSerialize]
        private Dictionary<int, I_ExtendedEffectArgumentBuilder> valueToBuilder;

        public void FillArguments(ExtendedEffectArgumentFiller filler, I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            int conditionalValueRes = conditionalValue.GetValue(owner, target, deliveryArguments);
            if (valueToBuilder.TryGetValue(conditionalValueRes, out I_ExtendedEffectArgumentBuilder builder))
            {
                builder.FillArguments(filler, owner, target, deliveryArguments);
            }
        }

        public ConditionalExtendedEffectArgumentBuilder(SerializationInfo info, StreamingContext context)
        {
            conditionalValue = StaticUtilities.LoadInterfaceValue<I_ConditionalExtendedEffectArgumentBuilderValue>(info, nameof(conditionalValue));
            valueToBuilder = StaticUtilities.LoadDictionary(
                info,
                nameof(valueToBuilder),
                (string name) =>
                {
                    return info.GetInt32(name);
                },
                (string name) =>
                {
                    return StaticUtilities.LoadInterfaceValue<I_ExtendedEffectArgumentBuilder>(info, name);
                }
            );
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(conditionalValue), conditionalValue);
            StaticUtilities.SaveDictionary(
                info,
                nameof(valueToBuilder),
                valueToBuilder,
                (string name, int key) =>
                {
                    info.AddValue(name, key);
                },
                (string name, I_ExtendedEffectArgumentBuilder value) =>
                {
                    StaticUtilities.SaveInterfaceValue(info, name, value);
                }
            );
        }
    }
}

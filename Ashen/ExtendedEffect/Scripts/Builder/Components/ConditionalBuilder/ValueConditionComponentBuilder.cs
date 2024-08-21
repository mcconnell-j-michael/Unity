using Ashen.DeliverySystem;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.ExtendedEffectSystem
{
    [Serializable]
    public class ValueConditionComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private I_DeliveryValue deliveryValue;
        [OdinSerialize]
        private Dictionary<int, I_ComponentBuilder> valueToEffect;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            float result = deliveryValue.Build(owner, target, deliveryArguments);
            int intValue = (int)result;
            if (valueToEffect.TryGetValue(intValue, out I_ComponentBuilder builder))
            {
                return builder.Build(owner, target, deliveryArguments);
            }
            return null;
        }

        public ValueConditionComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            deliveryValue = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(deliveryValue));
            valueToEffect = StaticUtilities.LoadDictionary(info, nameof(valueToEffect),
            (string name) =>
            {
                return info.GetInt32(name);
            },
            (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_ComponentBuilder>(info, name);
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(deliveryValue), deliveryValue);
            StaticUtilities.SaveDictionary(info, nameof(valueToEffect), valueToEffect,
                (string name, int key) =>
                {
                    info.AddValue(name, key);
                },
                (string name, I_ComponentBuilder value) =>
                {
                    StaticUtilities.SaveInterfaceValue(info, name, value);
                }
            );
        }
    }
}
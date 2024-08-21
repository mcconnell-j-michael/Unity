using Ashen.DeliverySystem;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;

namespace Ashen.ExtendedEffectSystem
{
    [Serializable]
    public class ExtendedEffectArgumentComponentBuilder : I_ComponentBuilder
    {
        [SerializeField]
        private ExtendedEffectArgument argument;
        [OdinSerialize]
        private Dictionary<int, I_ComponentBuilder> valueToEffect;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ExtendedEffectArgumentsPack packs = deliveryArguments.GetPack<ExtendedEffectArgumentsPack>();
            int intValue = packs.GetFloatArgumentFlat(argument);
            if (valueToEffect.TryGetValue(intValue, out I_ComponentBuilder builder))
            {
                return builder.Build(owner, target, deliveryArguments);
            }
            return null;
        }

        public ExtendedEffectArgumentComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            argument = ExtendedEffectArguments.Instance[info.GetInt32(nameof(argument))];
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
            info.AddValue(nameof(argument), (int)argument);
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

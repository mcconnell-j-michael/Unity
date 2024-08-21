using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ListEffect : I_Effect, ISerializable
    {
        public List<I_Effect> effects;

        public ListEffect()
        {
            effects = new List<I_Effect>();
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            for (int x = 0; x < effects.Count; x++)
            {
                effects[x].Apply(owner, target, targetDeliveryResult, deliveryArguments);
            }
        }

        public ListEffect(SerializationInfo info, StreamingContext context)
        {
            effects = StaticUtilities.LoadList(info, nameof(effects), (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_Effect>(info, name);
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(effects), effects, (string name, I_Effect effect) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, effect);
            });
        }
    }
}
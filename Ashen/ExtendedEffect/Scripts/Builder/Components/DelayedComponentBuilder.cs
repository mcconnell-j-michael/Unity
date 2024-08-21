using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class DelayedComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        public I_EffectBuilder effect;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            I_Effect effect = this.effect.Build(owner, target, deliveryArgument);
            if (effect == null)
            {
                return null;
            }
            return new DelayedComponent
            {
                effect = effect
            };
        }

        public DelayedComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            effect = StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, nameof(effect));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(effect), effect);
        }
    }
}
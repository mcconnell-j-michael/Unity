using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class EffectOverTimeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize, HideLabel, Indent]
        private I_EffectBuilder effect = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            I_Effect effect = this.effect.Build(owner, target, deliveryArguments);
            if (effect == null)
            {
                return null;
            }
            return new EffectOverTimeComponent(effect);
        }

        public EffectOverTimeComponentBuilder(SerializationInfo info, StreamingContext context)
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
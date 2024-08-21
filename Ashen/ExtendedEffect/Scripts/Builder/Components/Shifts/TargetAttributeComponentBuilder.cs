using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class TargetAttributeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private TargetAttribute attribute = default;
        [OdinSerialize]
        private int priority = default;
        [BoxGroup("Equation"), OdinSerialize, HideLabel]
        private Target target = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new TargetAttributeComponent(attribute, null, this.target, priority);
        }

        public TargetAttributeComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            attribute = TargetAttributes.Instance[info.GetInt32(nameof(attribute))];
            priority = info.GetInt32(nameof(priority));
            target = Targets.Instance[info.GetInt32(nameof(target))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(attribute), (int)attribute);
            info.AddValue(nameof(priority), priority);
            info.AddValue(nameof(target), target);
        }
    }
}
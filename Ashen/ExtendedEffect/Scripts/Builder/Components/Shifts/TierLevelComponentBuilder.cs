using Ashen.AbilitySystem;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class TierLevelComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private AbilityTag abilityTag = default;
        [OdinSerialize]
        private int priority = default;
        [OdinSerialize]
        private I_DeliveryValue tierLevel = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new TierLevelComponent(abilityTag, null, (int)tierLevel.Build(owner, target, deliveryArgument), priority);
        }

        public TierLevelComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            abilityTag = AbilityTags.Instance[info.GetInt32(nameof(abilityTag))];
            priority = info.GetInt32(nameof(priority));
            tierLevel = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(tierLevel));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(abilityTag), (int)abilityTag);
            info.AddValue(nameof(priority), priority);
            StaticUtilities.SaveInterfaceValue(info, nameof(tierLevel), tierLevel);
        }
    }
}
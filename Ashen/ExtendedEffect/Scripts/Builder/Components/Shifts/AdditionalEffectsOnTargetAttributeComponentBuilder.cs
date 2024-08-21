using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.ExtendedEffectSystem
{
    [Serializable]
    public class AdditionalEffectsOnTargetAttributeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private TargetAttribute attribute = default;
        [OdinSerialize]
        private int priority = default;
        [BoxGroup(nameof(effects)), OdinSerialize, HideLabel]
        private List<I_EffectBuilder> effects = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new AdditionalEffectsOnTargetAttributeComponent(attribute, null, effects, priority);
        }

        public AdditionalEffectsOnTargetAttributeComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            attribute = TargetAttributes.Instance[info.GetInt32(nameof(attribute))];
            priority = info.GetInt32(nameof(priority));
            effects = StaticUtilities.LoadList(info, nameof(effects), (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, name);
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(attribute), (int)attribute);
            info.AddValue(nameof(priority), priority);
            StaticUtilities.SaveList(info, nameof(effects), effects, (string name, I_EffectBuilder effect) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, effect);
            });
        }
    }
}

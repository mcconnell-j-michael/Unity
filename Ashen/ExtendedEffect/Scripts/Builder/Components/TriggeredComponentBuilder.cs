using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class TriggeredComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private ExtendedEffectTrigger[] triggers = default;
        [OdinSerialize, HideLabel, Indent]
        private I_EffectBuilder effect = default;
        [OdinSerialize, EnumToggleButtons]
        private TriggeredComponentBuilderLimiter limitType = TriggeredComponentBuilderLimiter.UNLIMITED;
        [OdinSerialize, ShowIf(nameof(limitType), TriggeredComponentBuilderLimiter.MULTIPLE), PropertyRange(1, 100)]
        private int limit = 1;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            int total = -1;
            if (limitType == TriggeredComponentBuilderLimiter.ONCE)
            {
                total = 1;
            }
            else if (limitType == TriggeredComponentBuilderLimiter.MULTIPLE)
            {
                total = limit;
            }
            return new TriggeredComponent(effect.Build(owner, target, deliveryArgument), triggers, total);
        }

        public TriggeredComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            triggers = StaticUtilities.LoadArray(info, nameof(triggers), (string name) =>
            {
                return ExtendedEffectTriggers.Instance[info.GetInt32(name)];
            });
            effect = StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, nameof(effect));
            limitType = (TriggeredComponentBuilderLimiter)info.GetValue(nameof(limitType), typeof(TriggeredComponentBuilderLimiter));
            limit = info.GetInt32(nameof(limit));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveArray(info, nameof(triggers), triggers, (string name, ExtendedEffectTrigger trigger) =>
            {
                info.AddValue(name, (int)trigger);
            });
            StaticUtilities.SaveInterfaceValue(info, nameof(effect), effect);
            info.AddValue(nameof(limitType), limitType);
            info.AddValue(nameof(limit), limit);
        }
    }

    [Serializable]
    public enum TriggeredComponentBuilderLimiter
    {
        UNLIMITED, ONCE, MULTIPLE
    }
}
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ashen.ExtendedEffectSystem
{
    [Serializable]
    public class AdditionalEffectsOnTargetAttributeComponent : A_ShiftableComponent<ShiftableAdditionalEffectsTool, TargetAttribute, List<I_EffectBuilder>>
    {
        public AdditionalEffectsOnTargetAttributeComponent() : base() { }
        public AdditionalEffectsOnTargetAttributeComponent(TargetAttribute enumValue, ShiftCategory shiftCategory, List<I_EffectBuilder> shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override TargetAttribute GetEnumFromIndex(int index)
        {
            return TargetAttributes.Instance[index];
        }

        protected override List<I_EffectBuilder> GetShift(SerializationInfo info, string serializationName)
        {
            return StaticUtilities.LoadList(info, serializationName, (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, name);
            });
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, List<I_EffectBuilder> shift)
        {
            StaticUtilities.SaveList(info, serializationName, shift, (string name, I_EffectBuilder effect) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, effect);
            });
        }

        public AdditionalEffectsOnTargetAttributeComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

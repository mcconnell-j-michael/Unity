using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ashen.ExtendedEffectSystem
{
    [Serializable]
    public class AdditionalSubAbilitiesOnTargetAttributeComponent : A_ShiftableComponent<ShiftableAdditionalSubAbilitiesTool, TargetAttribute, List<SubAbilityBuilder>>
    {
        public AdditionalSubAbilitiesOnTargetAttributeComponent() { }

        public AdditionalSubAbilitiesOnTargetAttributeComponent(TargetAttribute enumValue, ShiftCategory shiftCategory, List<SubAbilityBuilder> shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override TargetAttribute GetEnumFromIndex(int index)
        {
            return TargetAttributes.Instance[index];
        }

        protected override List<SubAbilityBuilder> GetShift(SerializationInfo info, string serializationName)
        {
            return StaticUtilities.LoadList(info, serializationName, (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<SubAbilityBuilder>(info, name);
            });
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, List<SubAbilityBuilder> shift)
        {
            StaticUtilities.SaveList(info, serializationName, shift, (string name, SubAbilityBuilder effect) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, effect);
            });
        }

        public AdditionalSubAbilitiesOnTargetAttributeComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

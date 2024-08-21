using Ashen.ExtendedEffectSystem;
using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    [Serializable]
    public class AttributeComponent : A_ShiftableComponent<AttributeTool, DerivedAttribute, I_DeliveryValue>, ISerializable
    {
        private bool mult;

        public AttributeComponent() : base() { }

        public AttributeComponent(DerivedAttribute enumValue, ShiftCategory shiftCategory, I_DeliveryValue shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override DerivedAttribute GetEnumFromIndex(int index)
        {
            return DerivedAttributes.Instance[index];
        }

        protected override I_DeliveryValue GetShift(SerializationInfo info, string serializationName)
        {
            return StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, serializationName);
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, I_DeliveryValue shift)
        {
            StaticUtilities.SaveInterfaceValue(info, serializationName, shift);
        }

        public AttributeComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
using Ashen.ExtendedEffectSystem;
using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    /**
     * This BaseStatusEffect will reduce or raise a characters 
     * Resistance for a certain period of time
     **/
    [Serializable]
    public class ResistanceComponent : A_ShiftableComponent<ResistanceTool, DamageType, int>, ISerializable
    {
        public ResistanceComponent() : base() { }

        public ResistanceComponent(DamageType enumValue, ShiftCategory shiftCategory, int shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override DamageType GetEnumFromIndex(int index)
        {
            return DamageTypes.Instance[index];
        }

        protected override int GetShift(SerializationInfo info, string serializationName)
        {
            return info.GetInt32(serializationName);
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, int shift)
        {
            info.AddValue(serializationName, shift);
        }

        public ResistanceComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
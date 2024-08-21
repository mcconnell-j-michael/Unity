using Ashen.ExtendedEffectSystem;
using Ashen.ToolSystem;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    public class TargetRangeAttributeComponent : A_ShiftableComponent<ShiftableTargetRangeTool, TargetAttribute, TargetRange>, ISerializable
    {
        public TargetRangeAttributeComponent() : base() { }

        public TargetRangeAttributeComponent(TargetAttribute enumValue, ShiftCategory shiftCategory, TargetRange shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override TargetAttribute GetEnumFromIndex(int index)
        {
            return TargetAttributes.Instance[index];
        }

        protected override TargetRange GetShift(SerializationInfo info, string serializationName)
        {
            return TargetRanges.Instance[info.GetInt32(serializationName)];
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, TargetRange shift)
        {
            info.AddValue(serializationName, (int)shift);
        }

        public TargetRangeAttributeComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

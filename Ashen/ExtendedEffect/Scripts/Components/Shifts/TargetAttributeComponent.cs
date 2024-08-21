using Ashen.AbilitySystem;
using Ashen.ExtendedEffectSystem;
using Ashen.ToolSystem;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    public class TargetAttributeComponent : A_ShiftableComponent<ShiftableTargetTool, TargetAttribute, Target>, ISerializable
    {
        public TargetAttributeComponent() : base() { }

        public TargetAttributeComponent(TargetAttribute enumValue, ShiftCategory shiftCategory, Target shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override TargetAttribute GetEnumFromIndex(int index)
        {
            return TargetAttributes.Instance[index];
        }

        protected override Target GetShift(SerializationInfo info, string serializationName)
        {
            return Targets.Instance[info.GetInt32(serializationName)];
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, Target shift)
        {
            info.AddValue(serializationName, (int)shift);
        }

        public TargetAttributeComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

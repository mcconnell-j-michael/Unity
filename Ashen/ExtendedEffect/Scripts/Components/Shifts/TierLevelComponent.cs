using Ashen.AbilitySystem;
using Ashen.ExtendedEffectSystem;
using Ashen.ToolSystem;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    public class TierLevelComponent : A_ShiftableComponent<ShiftableTierLevelTool, AbilityTag, int>, ISerializable
    {
        public TierLevelComponent() : base() { }

        public TierLevelComponent(AbilityTag enumValue, ShiftCategory shiftCategory, int shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override AbilityTag GetEnumFromIndex(int index)
        {
            return AbilityTags.Instance[index];
        }

        protected override int GetShift(SerializationInfo info, string serializationName)
        {
            return info.GetInt32(serializationName);
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, int shift)
        {
            info.AddValue(serializationName, shift);
        }

        public TierLevelComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

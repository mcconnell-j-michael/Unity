using Ashen.ExtendedEffectSystem;
using Ashen.ToolSystem;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    public class FacultyComponent : A_ShiftableComponent<FacultyTool, Faculty, bool>
    {
        public FacultyComponent() : base() { }

        public FacultyComponent(Faculty enumValue, ShiftCategory shiftCategory, bool shift, int priority) : base(enumValue, shiftCategory, shift, priority) { }

        protected override Faculty GetEnumFromIndex(int index)
        {
            return Faculties.Instance[index];
        }

        protected override bool GetShift(SerializationInfo info, string serializationName)
        {
            return info.GetBoolean(serializationName);
        }

        protected override void SaveShift(SerializationInfo info, string serializationName, bool shift)
        {
            info.AddValue(serializationName, shift);
        }

        public FacultyComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
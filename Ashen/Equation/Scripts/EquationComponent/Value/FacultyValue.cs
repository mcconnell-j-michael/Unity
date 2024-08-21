using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.EquationSystem
{
    public class FacultyValue : A_CacheableValue<FacultyTool, Faculty, bool>
    {
        public bool inverse = false;

        public FacultyValue() : base() { }

        protected override FacultyTool GetCachingTool(ToolManager toolManager)
        {
            return toolManager.Get<FacultyTool>();
        }

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            float res = base.Calculate(equation, source, target, total, extraArguments);
            if (inverse)
            {
                return res < .9f ? 1 : 0;
            }
            return res;
        }

        protected override Faculty GetEnumFromIndex(int index)
        {
            return Faculties.Instance[index];
        }

        public FacultyValue(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            inverse = info.GetBoolean(nameof(inverse));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            BaseGetObjectData(info, context);
            info.AddValue(nameof(inverse), inverse);
        }
    }
}
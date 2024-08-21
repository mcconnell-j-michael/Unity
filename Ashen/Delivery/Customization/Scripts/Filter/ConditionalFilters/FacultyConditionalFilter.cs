using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class FacultyConditionalFilter : A_ConditionalFilter
    {
        private Faculty faculty;
        private bool requireDisabled;

        public FacultyConditionalFilter(Faculty faculty, bool requireDisabled)
        {
            this.faculty = faculty;
            this.requireDisabled = requireDisabled;
        }

        public override bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            DeliveryTool dTool = target as DeliveryTool;
            if (!dTool)
            {
                return false;
            }
            FacultyTool fTool = dTool.toolManager.Get<FacultyTool>();
            if (!fTool)
            {
                return false;
            }
            bool enabled = fTool.Can(faculty);
            return (enabled && !requireDisabled) || (!enabled && requireDisabled);
        }

        public FacultyConditionalFilter(SerializationInfo info, StreamingContext context)
        {
            faculty = Faculties.Instance[info.GetInt32(nameof(faculty))];
            requireDisabled = info.GetBoolean(nameof(requireDisabled));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(faculty), (int)faculty);
            info.AddValue(nameof(requireDisabled), requireDisabled);
        }
    }
}
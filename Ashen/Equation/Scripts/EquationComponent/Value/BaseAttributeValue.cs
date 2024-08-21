using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.EquationSystem
{
    [Serializable]
    public class BaseAttributeValue : A_CacheableValue<BaseAttributeTool, BaseAttribute, float>
    {
        public BaseAttributeValue() : base() { }

        protected override BaseAttributeTool GetCachingTool(ToolManager toolManager)
        {
            return toolManager.Get<BaseAttributeTool>();
        }

        protected override BaseAttribute GetEnumFromIndex(int index)
        {
            return BaseAttributes.Instance[index];
        }

        public BaseAttributeValue(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            BaseGetObjectData(info, context);
        }
    }
}
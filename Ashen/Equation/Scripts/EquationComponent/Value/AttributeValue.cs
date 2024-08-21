using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.EquationSystem
{
    [Serializable]
    public class AttributeValue : A_CacheableValue<AttributeTool, DerivedAttribute, float>
    {
        public AttributeValue() : base() { }
        protected override AttributeTool GetCachingTool(ToolManager toolManager)
        {
            return toolManager.Get<AttributeTool>();
        }

        public AttributeValue(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            BaseGetObjectData(info, context);
        }

        protected override DerivedAttribute GetEnumFromIndex(int index)
        {
            return DerivedAttributes.Instance[index];
        }
    }
}
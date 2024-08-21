using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.EquationSystem
{
    [Serializable]
    public class ResourceValueValue : A_CacheableValue<ResourceValueTool, ResourceValue, float>
    {
        public ResourceValueValue() : base() { }

        protected override ResourceValueTool GetCachingTool(ToolManager toolManager)
        {
            return toolManager.Get<ResourceValueTool>();
        }

        public ResourceValueValue(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            BaseGetObjectData(info, context);
        }

        protected override ResourceValue GetEnumFromIndex(int index)
        {
            return ResourceValues.Instance[index];
        }
    }
}
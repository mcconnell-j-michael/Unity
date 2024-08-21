using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ResourceChangeEffect : I_Effect
    {
        private ResourceValue resourceValue;
        private int value;

        public ResourceChangeEffect(ResourceValue resourceValue, int value)
        {
            this.resourceValue = resourceValue;
            this.value = value;
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            ResourceChangeResult deliveryResult = targetDeliveryResult.GetResult<ResourceChangeResult>(DeliveryResultTypes.Instance.RESOURCE_CHANGE_RESULT);
            deliveryResult.AddResourceChange(resourceValue, value);
        }

        protected ResourceChangeEffect(SerializationInfo info, StreamingContext context)
        {
            value = (int)info.GetValue(nameof(value), typeof(int));
            resourceValue = ResourceValues.Instance[info.GetInt32(nameof(resourceValue))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(value), value);
            info.AddValue(nameof(resourceValue), (int)resourceValue);
        }
    }
}
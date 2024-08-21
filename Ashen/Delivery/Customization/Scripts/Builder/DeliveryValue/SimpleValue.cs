using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class SimpleValue : A_DeliveryValue
    {
        private float value;

        public SimpleValue(float value)
        {
            this.value = value;
        }

        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return value;
        }

        public override string Visualize()
        {
            return value.ToString();
        }

        public SimpleValue(SerializationInfo info, StreamingContext context)
        {
            value = info.GetInt32(nameof(value));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(value), value);
        }
    }
}
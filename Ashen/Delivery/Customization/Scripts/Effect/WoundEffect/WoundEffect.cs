using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class WoundEffect : I_Effect
    {
        public WoundEffect() { }
        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool deliveryTool = target as DeliveryTool;
            WoundTool woundTool = deliveryTool.toolManager.Get<WoundTool>();
            woundTool.ApplyRandomWound();
        }

        protected WoundEffect(SerializationInfo info, StreamingContext context)
        { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        { }
    }
}
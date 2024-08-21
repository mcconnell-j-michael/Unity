using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ResourceChangeEffectBuilder : I_EffectBuilder
    {
        public ResourceValue resourceValue;
        public I_DeliveryValue deliveryValue;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ToolManager tOwner = (owner as DeliveryTool).toolManager;
            ToolManager tTarget = (target as DeliveryTool).toolManager;
            int finalValue = (int)deliveryValue.Build(owner, target, deliveryArguments);
            ResourceValue rValue = resourceValue;
            return new ResourceChangeEffect(rValue, finalValue);
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += resourceValue.name + " += " + deliveryValue.Visualize();
            return vis;
        }

        public ResourceChangeEffectBuilder(SerializationInfo info, StreamingContext context)
        {
            resourceValue = ResourceValues.Instance[info.GetInt32(nameof(resourceValue))];
            deliveryValue = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(deliveryValue));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(resourceValue), (int)resourceValue);
            StaticUtilities.SaveInterfaceValue(info, nameof(deliveryValue), deliveryValue);
        }
    }
}
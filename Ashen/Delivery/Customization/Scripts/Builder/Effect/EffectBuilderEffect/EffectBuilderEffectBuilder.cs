using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class EffectBuilderEffectBuilder : I_EffectBuilder
    {
        public I_EffectBuilder effectBuilder;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new EffectBuilderEffect(effectBuilder);
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "{\n";
            vis += effectBuilder.visualize(depth + 1);
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "}";
            return vis;
        }

        public EffectBuilderEffectBuilder(SerializationInfo info, StreamingContext context)
        {
            effectBuilder = StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, nameof(effectBuilder));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(effectBuilder), effectBuilder);
        }
    }
}
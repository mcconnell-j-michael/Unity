using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ConditionalEffectBuilder : I_EffectBuilder
    {
        public I_EffectCondition effectCondition;
        public I_EffectBuilder effectResult;

        public ConditionalEffectBuilder() { }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            if (effectCondition.Check(owner, target, deliveryArguments))
            {
                return effectResult.Build(owner, target, deliveryArguments);
            }
            return null;
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "if(" + effectCondition.visualize() + ")\n";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "{\n" + effectResult.visualize(depth + 1) + "\n";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "}";
            return visualization;
        }

        public ConditionalEffectBuilder(SerializationInfo info, StreamingContext context)
        {
            effectCondition = StaticUtilities.LoadInterfaceValue<I_EffectCondition>(info, nameof(effectCondition));
            effectResult = StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, nameof(effectResult));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(effectCondition), effectCondition);
            StaticUtilities.SaveInterfaceValue(info, nameof(effectResult), effectResult);
        }
    }
}
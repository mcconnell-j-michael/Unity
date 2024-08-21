using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    public class ValueCondition : I_TagOperation
    {
        [SerializeField]
        private I_DeliveryValue value;
        [OdinSerialize]
        private Dictionary<int, I_TagOperation> valueToEffect;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            float result = value.Build(owner, target, deliveryArguments);
            int intValue = (int)result;
            if (valueToEffect.TryGetValue(intValue, out I_TagOperation operation))
            {
                operation.Operate(owner, target, tagState, deliveryArguments);
                return;
            }
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "switch(" + value.Visualize() + ") {\n";
            depth += 1;
            foreach (KeyValuePair<int, I_TagOperation> pair in valueToEffect)
            {
                for (int x = 0; x < depth; x++)
                {
                    visualization += "\t";
                }
                visualization += pair.Key + ":\n" + pair.Value.visualize(depth + 1);
                visualization += "\n";

            }
            depth -= 1;
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "}";
            return visualization;
        }
    }
}

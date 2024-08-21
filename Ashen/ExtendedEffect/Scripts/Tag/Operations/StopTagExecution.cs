using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class StopTagExecution : I_TagOperation
    {
        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            tagState.continueOperation = false;
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "END";
            return visualization;
        }
    }
}
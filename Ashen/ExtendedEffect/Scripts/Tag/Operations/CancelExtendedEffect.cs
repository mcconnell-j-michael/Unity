using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class CancelExtendedEffect : I_TagOperation
    {
        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            tagState.validStatusEffect = false;
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            return visualization + "Cancel Status Effect";
        }
    }
}
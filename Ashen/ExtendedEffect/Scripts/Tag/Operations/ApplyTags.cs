using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public class ApplyTags : I_TagOperation
    {
        public List<ExtendedEffectTag> tags;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            StatusTool statusTool = ((DeliveryTool)target).toolManager.Get<StatusTool>();
            
            tagState.appliedTags.AddRange(tags);
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "Add Tags: [";


            for (int x = 0; x < tags.Count; x++)
            {
                visualization += tags[x].ToString();
                if (x != tags.Count-1)
                {
                    visualization += ", ";
                }
            }
            visualization += "]";
            return visualization;
        }
    }
}
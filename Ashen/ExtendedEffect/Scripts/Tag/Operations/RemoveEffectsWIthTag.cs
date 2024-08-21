using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class RemoveEffectsWIthTag : I_TagOperation
    {
        [AutoPopulate, EnumSODropdown]
        public List<ExtendedEffectTag> tags;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool tDeliveryTool = target as DeliveryTool;
            if (tDeliveryTool)
            {
                StatusTool tStatusTool = tDeliveryTool.toolManager.Get<StatusTool>();
                if (tStatusTool)
                {
                    foreach (ExtendedEffectTag tag in tags)
                    {
                        tStatusTool.DisableAllStatusEffectsWithTag(tag);
                    }
                }
            }
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "Disable StatusEffects with tags: [";
            if (tags == null)
            {
                return visualization + "]";
            }
            int count = 0;
            foreach (ExtendedEffectTag tag in tags)
            {
                if (count != 0)
                {
                    visualization += ", ";
                }
                visualization += tag.ToString();
                count++;
            }
            return visualization + "]";
        }
    }
}
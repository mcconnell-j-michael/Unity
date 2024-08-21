using Ashen.ToolSystem;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class TagChecker : I_TagConditional
    {
        [HideLabel, Title("Tag")]
        public ExtendedEffectTag tag;
        [HideLabel, Title("Negate")]
        public bool negate;

        public bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool tDeliveryTool = target as DeliveryTool;
            if (tDeliveryTool)
            {
                StatusTool tStatusTool = tDeliveryTool.toolManager.Get<StatusTool>();
                if (tStatusTool)
                {
                    bool res = tStatusTool.CheckStatusEffectTag(tag);
                    return negate ? !res : res;
                }
            }
            return false;
        }

        public string visualize()
        {
            string visualization = "";
            if (negate)
            {
                visualization += "!";
            }
            visualization += "Has " + tag.ToString();
            return visualization;
        }
    }

}
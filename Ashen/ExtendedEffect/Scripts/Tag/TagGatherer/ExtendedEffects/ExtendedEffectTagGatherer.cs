using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.ExtendedEffectSystem
{
    public class ExtendedEffectTagGatherer : A_TagGatherer<ExtendedEffect>
    {
        [HideLabel, FoldoutGroup("Tags To Gather"), InlineProperty]
        public List<ExtendedEffectTag> tags;

        protected override List<ExtendedEffect> Gather(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool tDeliveryTool = target as DeliveryTool;
            if (tDeliveryTool)
            {
                StatusTool tStatusTool = tDeliveryTool.toolManager.Get<StatusTool>();
                if (tStatusTool)
                {
                    return tStatusTool.GetExtendedEffectsWithTags(tags);
                }
            }
            return null;
        }

        protected override string VisualizeInternal(int depth)
        {
            string visualization = "Extended Effects with Tags: ";
            bool initial = true;
            foreach (ExtendedEffectTag tag in tags)
            {
                if (initial)
                {
                    visualization += tag.name;
                }
                else
                {
                    visualization += ", " + tag.name;
                }
            }
            return visualization;
        }
    }
}

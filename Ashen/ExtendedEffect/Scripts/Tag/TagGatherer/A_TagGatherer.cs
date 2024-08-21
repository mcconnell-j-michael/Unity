using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.ExtendedEffectSystem
{
    public abstract class A_TagGatherer<T> : I_TagOperation
    {
        [OdinSerialize]
        [HideLabel, FoldoutGroup("OnGathered"), InlineProperty]
        private A_TagGatheredListener<T> operation;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            List<T> foundItems = Gather(owner, target, tagState, deliveryArguments);
            if (foundItems != null)
            {
                operation.OnGathered(owner, target, tagState, deliveryArguments, foundItems);
            }
        }

        protected abstract List<T> Gather(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments);
        protected abstract string VisualizeInternal(int depth);

        public string visualize(int depth)
        {
            string visualization = StaticUtilities.GetTabs(depth) + "Gather[" + VisualizeInternal(depth) + "]\n";
            visualization += StaticUtilities.GetTabs(depth) + "{\n";
            visualization += operation.Visualize(depth + 1) + "\n";
            visualization += StaticUtilities.GetTabs(depth) + "}";
            return visualization;
        }
    }
}

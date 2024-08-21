using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ExtendedEffectSystem
{
    public abstract class A_TagGatheredListener<T>
    {
        public void OnGathered(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments, List<T> gatheredInfo)
        {
            if (gatheredInfo == null || gatheredInfo.Count == 0)
            {
                return;
            }
            OnGatheredInternal(owner, target, tagState, deliveryArguments, gatheredInfo);
        }

        protected abstract void OnGatheredInternal(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments, List<T> gatheredInfo);

        public abstract string Visualize(int depth);
    }
}

using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ExtendedEffectSystem
{
    public class ResetTimerTagGatheredListener : A_TagGatheredListener<ExtendedEffect>
    {
        protected override void OnGatheredInternal(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments, List<ExtendedEffect> gatheredInfo)
        {
            foreach (ExtendedEffect effect in gatheredInfo)
            {
                effect.Ticker.Reset();
            }
        }

        public override string Visualize(int depth)
        {
            return StaticUtilities.GetTabs(depth) + "Reset Timer";
        }
    }
}

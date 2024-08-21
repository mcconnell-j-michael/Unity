using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class EffectsProcessor : I_DeliveryProcessor
    {
        public static readonly string IGNORE_OWNER = "ignoreOwner";

        public void Process(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            EffectsArgumentPack effectsArgumentPack = deliveryArguments.GetPack<EffectsArgumentPack>();
            DeliveryResultsArgumentPack deliveryResultsArgumentPack = deliveryArguments.GetPack<DeliveryResultsArgumentPack>();
            DeliveryResultPack drp = deliveryResultsArgumentPack.GetDeliveryResultPack();
            if (owner != target || !deliveryArguments.IsTrue(IGNORE_OWNER))
            {
                drp.empty = false;
                foreach (I_Effect effect in effectsArgumentPack.GetEffects())
                {
                    effect.Apply(owner, target, drp, deliveryArguments);
                }
            }
        }
    }
}
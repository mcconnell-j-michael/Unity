using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * This class is used as a simple plug into the Ashen.DeliverySystem
     * It will only contain static methods that will initiate the delivery of
     * a DeliveryContainer
     */
    public class DeliveryUtility
    {
        public static void Deliver(I_DeliveryPack deliveryPack, I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryArgumentPacks deliveryArguments)
        {
            deliveryPack.FillArguments(dOwner, dTarget, deliveryArguments);
            deliveryPack.Apply(dOwner, dTarget, deliveryArguments);
        }

        public static List<I_ExtendedEffect> DeliverGems(I_DeliveryPack deliveryPack, I_DeliveryTool dTarget, DeliveryArgumentPacks deliveryArguments)
        {
            deliveryPack.FillArguments(dTarget, dTarget, deliveryArguments);
            deliveryPack.Apply(dTarget, dTarget, deliveryArguments);
            DeliveryResultPack pack = deliveryArguments.GetPack<DeliveryResultsArgumentPack>().GetDeliveryResultPack();
            StatusEffectResult result = pack.GetResult<StatusEffectResult>(DeliveryResultTypes.Instance.STATUS_EFFECT_RESULT_TYPE);
            List<I_ExtendedEffect> effects = new List<I_ExtendedEffect>();
            effects.AddRange(result.AppliedStatusEffects);
            return effects;
        }
    }
}

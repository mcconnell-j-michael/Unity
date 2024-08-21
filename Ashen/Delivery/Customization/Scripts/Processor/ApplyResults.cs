using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class ApplyResults : I_DeliveryProcessor
    {
        public void Process(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryResultsArgumentPack resultPack = deliveryArguments.GetPack<DeliveryResultsArgumentPack>();
            resultPack.GetDeliveryResultPack().Deliver(owner, target, deliveryArguments);
        }
    }
}
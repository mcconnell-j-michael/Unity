using UnityEngine;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(DeliveryResultType), menuName = "Custom/Enums/" + nameof(DeliveryResultTypes) + "/Type")]
    public class DeliveryResultType : A_EnumSO<DeliveryResultType, DeliveryResultTypes>
    {
        public A_DeliveryResult deliveryResult;
    }
}
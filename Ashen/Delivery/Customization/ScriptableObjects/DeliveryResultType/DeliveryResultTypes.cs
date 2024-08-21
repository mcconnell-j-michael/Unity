using UnityEngine;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(DeliveryResultTypes), menuName = "Custom/Enums/" + nameof(DeliveryResultTypes) + "/Types")]
    public class DeliveryResultTypes : A_EnumList<DeliveryResultType, DeliveryResultTypes>
    {
        public DeliveryResultType DAMAGE_RESULT_TYPE;
        public DeliveryResultType STATUS_EFFECT_RESULT_TYPE;
        public DeliveryResultType CRITICAL_RESULT_TYPE;
        public DeliveryResultType RESOURCE_CHANGE_RESULT;
    }
}
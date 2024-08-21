using Sirenix.OdinInspector;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(DeliveryProcess), menuName = "Custom/CombatInfrastructure/" + nameof(DeliveryProcess))]
    public class DeliveryProcess : SerializedScriptableObject
    {
        public I_DeliveryProcessor processor;
    }
}
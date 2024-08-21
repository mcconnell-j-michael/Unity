using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A status trigger defines when a status can proc. i.e. a damage over time effect can proc
     * every few seconds when the status effect ticks. A status effect could cause a character 
     * damagage every time they attack. 
     **/
    [CreateAssetMenu(fileName = nameof(ExtendedEffectTrigger), menuName = "Custom/Enums/" + nameof(ExtendedEffectTriggers) + "/Type")]
    public class ExtendedEffectTrigger : A_EnumSO<ExtendedEffectTrigger, ExtendedEffectTriggers>
    {
        public bool shouldRegister;
    }
}
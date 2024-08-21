using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * The scriptable object used to build a deliveryPack
     **/
    [CreateAssetMenu(fileName = "DeliveryPack", menuName = "Custom/CombatInfrastructure/DeliveryPack", order = 1)]
    public class DeliveryPackScriptableObject : SerializedScriptableObject
    {

        [ReadOnly, HideLabel, FoldoutGroup("visualization")]
        [InfoBox("$visualize")]
        public string visualize;

        [HideLabel, FoldoutGroup("Effect"), InlineProperty, Indent]
        public I_EffectBuilder deliveryPack;

        public List<Description> descriptions;

        [Button]
        public void visualizeHandler()
        {
            visualize = deliveryPack.visualize(0);
        }
    }
}
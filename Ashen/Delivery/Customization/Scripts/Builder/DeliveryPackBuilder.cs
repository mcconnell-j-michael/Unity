using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class DeliveryPackBuilder
    {
        [ReadOnly, HideLabel, FoldoutGroup("visualization")]
        [InfoBox("$visualize")]
        public string visualize;

        [HideLabel, FoldoutGroup("Effect"), InlineProperty, Indent]
        public I_EffectBuilder deliveryPack;

        [FoldoutGroup("Filters"), HideLabel, Title("Pre")]
        public I_FilterBuilder preFilters;
        [FoldoutGroup("Filters"), HideLabel, Title("Post")]
        public I_FilterBuilder postFilters;

        [Button]
        public void visualizeHandler()
        {
            visualize = deliveryPack.visualize(0);
        }
    }
}
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;
using System;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ThresholdTriggerEventBuilder
    {
        [EnumToggleButtons, HideLabel]
        public ThresholdTriggerEventType type;

        [ShowIf(nameof(type), ThresholdTriggerEventType.VALUE)]
        public I_Equation value;
    }

    public enum ThresholdTriggerEventType
    {
        MAX, MIN, VALUE
    }
}
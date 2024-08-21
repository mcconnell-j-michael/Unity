using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A number shift allows for values to be scaled. This is useful for things like
     * Resistances being temporarily boosted or reduced by StatusEffects.
     **/
    public class NumberShift
    {
        public NumberShift()
        {
            value = 0;
        }

        [OdinSerialize]
        public float value;

        public float Value()
        {
            return value;
        }
    }
}
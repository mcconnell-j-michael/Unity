using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace Ashen.ToolSystem
{
    [Serializable]
    public class ShiftLimit
    {
        [OdinSerialize, ToggleGroup(nameof(enableMinimum))]
        private bool enableMinimum;
        [OdinSerialize, ToggleGroup(nameof(enableMinimum))]
        private float minimum;

        [OdinSerialize, ToggleGroup(nameof(enableMaximum))]
        private bool enableMaximum;
        [OdinSerialize, ToggleGroup(nameof(enableMaximum))]
        private float maximum;

        public float Limit(float original)
        {
            float newVal = original;
            if (enableMinimum && newVal < minimum)
            {
                newVal = minimum;
            }
            if (enableMaximum && newVal > maximum)
            {
                newVal = maximum;
            }
            return newVal;
        }
    }
}

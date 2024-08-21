using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public class SingleSliderManager : MonoBehaviour, I_SliderManager
    {
        [SerializeField]
        private Slider resourceSlider;
        [SerializeField]
        private bool reverseFill;

        public void ResetFill()
        {
            resourceSlider.value = reverseFill ? 0f : 1f;
        }

        public void UpdateFill(ToolManager toolManager, ThresholdEventValue value)
        {
            float currentPercentage = (float)value.currentValue / value.maxValue;
            resourceSlider.value = reverseFill ? (1f - currentPercentage) : currentPercentage;
        }
    }
}
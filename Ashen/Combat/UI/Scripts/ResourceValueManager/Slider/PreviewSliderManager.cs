using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public class PreviewSliderManager : MonoBehaviour, I_SliderManager
    {
        [SerializeField]
        private Slider resourceSlider;
        [SerializeField]
        private Slider previewSlider;
        [SerializeField]
        private Slider promisedSlider;
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
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            int promised = value.tempValues[(int)ThresholdValueTempCategories.Instance.PROMISED];
            int promisedValue = rvTool.CalculateLimit(value.resourceValue, promised);
            float promisedPercentage = (float)promisedValue / value.maxValue;
            promisedSlider.value = reverseFill ? (1f - promisedPercentage) : promisedPercentage;
        }
    }
}
using Ashen.DeliverySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public class ResourceDialManager : A_ResourceManager
    {
        [SerializeField]
        private Image resourceDial;
        [SerializeField]
        private bool inverseFill;

        protected override void UpdateFill(ThresholdEventValue value)
        {
            float percentage = value.currentValue / ((float)value.maxValue);
            resourceDial.fillAmount = inverseFill ? 1 - percentage : percentage;
        }

        protected override void InternalUnregisterToolManager()
        {
            resourceDial.fillAmount = 0;
        }
    }
}
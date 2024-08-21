using Ashen.DeliverySystem;
using TMPro;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class NamedResourceBarManager : ResourceBarManager
    {
        [SerializeField]
        private TextMeshProUGUI resourceAmountText;
        [SerializeField]
        protected TextMeshProUGUI resourceNameText;
        [SerializeField]
        protected string defaultName;

        protected override void UpdateFill(ThresholdEventValue value)
        {
            base.UpdateFill(value);
            resourceAmountText.text = (value.currentValue) + "";
            resourceNameText.name = defaultName;
        }

        protected override void InternalUnregisterToolManager()
        {
            base.InternalUnregisterToolManager();
            resourceNameText.text = "";
            resourceAmountText.text = "";
        }
    }
}
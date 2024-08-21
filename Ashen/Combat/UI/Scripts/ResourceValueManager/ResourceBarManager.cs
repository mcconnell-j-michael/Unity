using Ashen.CombatSystem;
using Ashen.DeliverySystem;
using JoshH.UI;
using UnityEngine;

public class ResourceBarManager : A_ResourceManager, I_ThresholdListener
{
    [SerializeField]
    private I_SliderManager sliderManager;
    [SerializeField]
    protected UIGradient gradient;
    [SerializeField]
    private bool reverseFill;

    private void Awake()
    {
        sliderManager = GetComponent<I_SliderManager>();
    }

    protected override void UpdateFill(ThresholdEventValue value)
    {
        sliderManager.UpdateFill(toolManager, value);
    }

    protected override void InternalUnregisterToolManager()
    {
        sliderManager.ResetFill();
    }
}

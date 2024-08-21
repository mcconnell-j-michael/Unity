using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour, I_ThresholdListener
{
    public Image bar;
    public Image background;
    public ToolManager toolManager;
    public ResourceValue resourceValue;
    public RectTransform parentTransform;
    public BarCanvasManager barCanvasManager;
    private ResourceValueTool damageTool;

    public void Start()
    {
        damageTool = toolManager.Get<ResourceValueTool>();
        damageTool.RegiserThresholdChangeListener(resourceValue, this);
        bar.fillAmount = damageTool.GetCurrentPercentage(resourceValue);
    }

    public void OnDestroy()
    {
        if (damageTool)
        {
            damageTool.UnRegesterThresholdChangeListener(resourceValue, this);
        }
    }

    public void OnThresholdEvent(ThresholdEventValue value)
    {
        if (value.previousValue == 0)
        {
            barCanvasManager.AddBar(resourceValue);
        }
        if (value.currentValue == 0)
        {
            barCanvasManager.RemoveBar(resourceValue);
        }
        float percentage = ((float)value.currentValue / value.maxValue);
        if (bar)
        {
            bar.fillAmount = percentage;
        }
    }
}

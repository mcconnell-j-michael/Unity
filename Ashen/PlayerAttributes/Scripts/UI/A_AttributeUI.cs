using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using TMPro;
using UnityEngine;

public abstract class A_AttributeUI<Enum> : MonoBehaviour, I_EnumCacheable
    where Enum : I_EnumSO
{
    public TextMeshProUGUI text;
    public string overrideName;
    public TooltipTrigger tooltipTrigger;

    public void Recalculate(I_EnumSO enumValue, I_DeliveryTool toolManager)
    {
        SetText();
    }

    public void SetText()
    {
        string name = "";
        if (overrideName != null && overrideName != "")
        {
            name += overrideName;
        }
        else
        {
            name += GetDefaultName();
        }
        text.text = name + ": " + GetValue();
        SetTooltip();
    }

    public void SetTooltip()
    {
        tooltipTrigger.content = "Base: " + GetBaseValue() + "\nBonus: " + GetValue();
    }

    public abstract string GetDefaultName();
    public abstract float GetValue();
    public abstract float GetBaseValue();
}

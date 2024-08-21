using System;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectSymbolManager : MonoBehaviour, I_StatusEffectSymbolListener
{
    public Image image;
    public Image backgroundImage;
    public RectTransform parentTransform;
    [NonSerialized]
    public StatusEffectSymbol symbol;
    [NonSerialized]
    public StatusEffectSymbolsManager statusEffectSymbolsManager;

    public void OnStatusEffectSymbolEvent(StatusEffectSymbolEventValue value)
    {
        if (value.ended)
        {
            statusEffectSymbolsManager.Disable(this);
        }
        image.fillAmount = value.percentage;
    }
}
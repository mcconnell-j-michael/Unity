using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class A_ColorThemeBinder : MonoBehaviour
{
    [ShowInInspector]
    protected ColorThemeManager colorThemeManager;

    protected abstract void InternalUpdateColor();

    public void UpdateColor()
    {
        if (!colorThemeManager)
        {
            colorThemeManager = gameObject.GetComponentInParent<ColorThemeManager>();
        }
        if (!colorThemeManager)
        {
            throw new System.Exception("Please add an instance of " + nameof(ColorThemeManager) + " to the object heirarchy");
        }
        InternalUpdateColor();
    }

    public List<string> GetColorThemes()
    {
        if (!colorThemeManager || colorThemeManager.colorMap == null)
        {
            return null;
        }
        List<string> colorOptions = new List<string>();
        colorOptions.AddRange(colorThemeManager.colorMap.Keys);
        return colorOptions;
    }
}

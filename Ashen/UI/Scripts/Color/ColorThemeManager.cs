using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorThemeManager : SerializedMonoBehaviour
{
    public Dictionary<string, Color> colorMap;

    private Graphic graphic;

#if UNITY_EDITOR
    private void Update()
    {
        A_ColorThemeBinder[] colorThemeBinders = gameObject.GetComponentsInChildren<A_ColorThemeBinder>();
        foreach (A_ColorThemeBinder binder in colorThemeBinders)
        {
            binder.UpdateColor();
        }
    }
#endif
}

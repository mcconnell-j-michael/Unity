using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;

[Serializable]
public class ActionOptionColorScheme
{
    [HideLabel, Title("Border")]
    public Color border;
    [HideLabel, Title("Background")]
    public Color background;
    [HideLabel, Title("Title")]
    public Color title;

    [BoxGroup("Selected"), HideLabel]
    public Color color1;
    [BoxGroup("Selected"), HideLabel]
    public Color color2;
}

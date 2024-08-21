using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class SkillColorTheme
{
    [HideLabel, Title("Background")]
    public Color background;
    [HideLabel, Title("name")]
    public Color name;
    [HideLabel, Title("cost")]
    public Color cost;

    [BoxGroup("Selected"), HideLabel]
    public Color color1;
    [BoxGroup("Selected"), HideLabel]
    public Color color2;
}

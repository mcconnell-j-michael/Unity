using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class SkillTreeNodeColorScheme
{
    [HideLabel, Title("Background, Name Text, Current Value Background, Total Text")]
    public Color one;
    [HideLabel, Title("Name Text Background, Current Value Text")]
    public Color two;
    [HideLabel, Title("Total Background")]
    public Color three;
}

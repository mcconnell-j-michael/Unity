using Ashen.AbilitySystem;
using JoshH.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelector : MonoBehaviour
{
    public UIGradient gradient;
    public Ability ability;
    public SkillPanelHandler skillPanel;
    public TextMeshProUGUI skillCost;
    public TextMeshProUGUI skillName;
    public Image background;


    public bool Valid
    {
        set
        {
            if (value)
            {
                background.color = skillPanel.validOption.background;
                gradient.LinearColor1 = skillPanel.validOption.color1;
                gradient.LinearColor2 = skillPanel.validOption.color2;
                skillName.color = skillPanel.validOption.name;
                skillCost.color = skillPanel.validOption.cost;
            }
            else
            {
                background.color = skillPanel.invalidOption.background;
                gradient.LinearColor1 = skillPanel.invalidOption.color1;
                gradient.LinearColor2 = skillPanel.invalidOption.color2;
                skillName.color = skillPanel.invalidOption.name;
                skillCost.color = skillPanel.invalidOption.cost;
            }
        }
    }

    public void Deselected()
    { }

    public void GradientEnabled(bool enabled)
    {
        gradient.enabled = enabled;
    }
}

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public class InfusionPanelSelector : A_Selector
    {
        [Hide, FoldoutGroup("Color Scheme"), Title("Valid"), SerializeField]
        private ActionOptionColorScheme validOption;
        [Hide, FoldoutGroup("Color Scheme"), Title("Invalid"), SerializeField]
        private ActionOptionColorScheme invalidOption;

        [SerializeField]
        private Image border;
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private bool infuse;
        public bool Infuse { get { return infuse; } }

        protected override void OnValid()
        {
            border.color = validOption.border;
            background.color = validOption.background;
            title.color = validOption.title;
            gradient.LinearColor1 = validOption.color1;
            gradient.LinearColor2 = validOption.color2;
        }

        protected override void OnInValid()
        {
            border.color = invalidOption.border;
            background.color = invalidOption.background;
            title.color = invalidOption.title;
            gradient.LinearColor1 = invalidOption.color1;
            gradient.LinearColor2 = invalidOption.color2;
        }
    }
}
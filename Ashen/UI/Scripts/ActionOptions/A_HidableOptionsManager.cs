using Sirenix.OdinInspector;

namespace Ashen.UISystem
{
    public abstract class A_HidableOptionsManager<OptionsManager, OptionsUI> : A_OptionsManager<OptionsManager, OptionsUI>
        where OptionsManager : A_HidableOptionsManager<OptionsManager, OptionsUI>
        where OptionsUI : A_HidableOptionUI
    {
        [Hide, FoldoutGroup("Color Scheme"), Title("Valid")]
        public ActionOptionColorScheme validOption;
        [Hide, FoldoutGroup("Color Scheme"), Title("Invalid")]
        public ActionOptionColorScheme invalidOption;

        public void HideCurrent()
        {
            if (currentlySelected)
            {
                currentlySelected.HideSelected();
            }
        }

        public void ShowCurrent()
        {
            if (currentlySelected)
            {
                currentlySelected.ShowSelected();
            }
        }
    }
}

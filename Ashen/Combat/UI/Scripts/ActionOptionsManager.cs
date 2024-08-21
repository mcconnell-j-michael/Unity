using Ashen.UISystem;
using Sirenix.OdinInspector;

public class ActionOptionsManager : A_OptionsManager<ActionOptionsManager, CombatOptionUI>
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

    public CombatOptionUI[] GetCombatOptionsInOrder()
    {
        return optionsInOrder;
    }
}

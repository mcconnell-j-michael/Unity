using Ashen.UISystem;

namespace Ashen.StartMenuSystem
{
    public class StartMenuOptionUI : A_OptionUI
    {
        protected override I_OptionsManager GetManager()
        {
            return StartMenuOptionsManager.Instance;
        }
    }
}
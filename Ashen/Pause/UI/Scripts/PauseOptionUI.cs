using Ashen.UISystem;

namespace Ashen.PauseSystem
{
    public class PauseOptionUI : A_HidableOptionUI
    {
        protected override I_OptionsManager GetManager()
        {
            return PauseOptionsManager.Instance;
        }
    }
}
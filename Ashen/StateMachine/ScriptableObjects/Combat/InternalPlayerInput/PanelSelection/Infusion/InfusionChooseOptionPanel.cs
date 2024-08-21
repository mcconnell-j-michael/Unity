using Ashen.CombatSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class InfusionChooseOptionPanel : A_ChooseSelectorPanel<InfusionSelectorPanelHandler, InfusionPanelSelector>
    {
        public InfusionChooseOptionPanel(I_GameState previousState) : base(previousState) { }

        public override IEnumerator GetPanelHandler()
        {
            panelHandler = InfusionSelectorPanelHandler.Instance;
            panelHandler.activeAbilities = 2;
            yield break;
        }

        public override void OnSubmitInternal()
        {
            InfusionPanelSelector current = selectors[currentSelectorIdx];
            response.nextState = new ChooseInfusion(this, current.Infuse);
        }
    }
}

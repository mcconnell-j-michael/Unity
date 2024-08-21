using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class ChooseInfusion : A_PlayerInputState
    {
        private I_GameState previousState;

        private CombatInfusion currentlySelected = null;
        private InfusionSelectorManager infusionManager;
        private PartyMemberManager partyMemberManager;
        private bool infuse;

        public ChooseInfusion(I_GameState previousState, bool infuse)
        {
            this.infuse = infuse;
            this.previousState = previousState;
        }

        protected override IEnumerator PreProcessState()
        {
            PlayerInputState inputState = PlayerInputState.Instance;
            PlayerPartyManager playerPartyManager = PlayerPartyHolder.Instance.partyManager;
            PartyPosition pos = playerPartyManager.GetPosition(inputState.currentlySelected);
            partyMemberManager = playerPartyManager.GetPartyUIManager().GetCharacterSelector(pos) as PartyMemberManager;
            infusionManager = partyMemberManager.InfusionSelectorManager;
            infusionManager.StartSelection(infuse);
            CombatInfusion next = (currentlySelected == null) ? infusionManager.First : currentlySelected;
            currentlySelected = null;
            Select(next);
            yield break;
        }

        public override void OnSubmit()
        {
            if (!infusionManager.CanBeSubmitted(currentlySelected))
            {
                return;
            }
            AbilitySO toBuild = infuse ?
                infusionManager.GetInfusionEffect(currentlySelected) :
                infusionManager.GetDiffusionEffect(currentlySelected);
            if (toBuild != null)
            {
                StopSelecting();
                response.nextState = new ChooseTargetCombat(this, toBuild.builder.Build());
            }
            else
            {
                OnCancel();
            }
        }

        public override void OnCancel()
        {
            StopSelecting();
            response.nextState = previousState;
        }

        public override void OnSelectLeft()
        {
            if (IsDelayed())
            {
                return;
            }
            CombatInfusion next = infusionManager.GetLeft(currentlySelected);
            Select(next);
        }

        public override void OnSelectRight()
        {
            if (IsDelayed())
            {
                return;
            }
            CombatInfusion next = infusionManager.GetRight(currentlySelected);
            Select(next);
        }

        public override void OnSelectUp()
        {
            if (IsDelayed())
            {
                return;
            }
            CombatInfusion next = infusionManager.GetUp(currentlySelected);
            Select(next);
        }

        public override void OnSelectDown()
        {
            if (IsDelayed())
            {
                return;
            }
            CombatInfusion next = infusionManager.GetDown(currentlySelected);
            Select(next);
        }

        private void Select(CombatInfusion next)
        {
            if (next == currentlySelected)
            {
                return;
            }
            SetSelectDelay(0.15f);
            if (currentlySelected != null)
            {
                infusionManager.Deselect(currentlySelected);
            }
            infusionManager.Select(next);
            currentlySelected = next;
            partyMemberManager.DisplayPlayerText(currentlySelected.name);
        }

        private void StopSelecting()
        {
            infusionManager.StopSelection();
            infusionManager = null;
            currentlySelected = null;
            partyMemberManager.HidePlayerText();
        }
    }
}

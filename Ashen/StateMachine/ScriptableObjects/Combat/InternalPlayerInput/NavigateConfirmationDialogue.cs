using Ashen.CombatSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class NavigateConfirmationDialogue : A_PlayerInputState
    {
        private ConfirmationBoxManager confirmationBoxManager;
        private string dialogueText;

        private I_GameState confirmGameState;
        private I_GameState negativeGameState;

        public NavigateConfirmationDialogue(string dialogueText, I_GameState confirmGameState, I_GameState negativeGameState)
        {
            this.confirmationBoxManager = ConfirmationBoxManager.Instance;
            this.dialogueText = dialogueText;
            this.confirmGameState = confirmGameState;
            this.negativeGameState = negativeGameState;
        }

        protected override IEnumerator PreProcessState()
        {
            confirmationBoxManager.Initialize(dialogueText);
            yield break;
        }

        protected override void PostProcessState()
        {
            confirmationBoxManager.DeInitialize();
        }

        public override void OnSelectLeft()
        {
            if (IsDelayed())
            {
                return;
            }
            SetSelectDelay(0.15f);
            confirmationBoxManager.SelectNext();
        }

        public override void OnSelectRight()
        {
            if (IsDelayed())
            {
                return;
            }
            SetSelectDelay(0.15f);
            confirmationBoxManager.SelectNext();
        }

        public override void OnSubmit()
        {
            if (confirmationBoxManager.Submit() == ConfirmationButtonValue.YES)
            {
                response.nextState = confirmGameState;
            }
            else
            {
                response.nextState = negativeGameState;
            }
        }

        public override void OnCancel()
        {
            confirmationBoxManager.Select(ConfirmationButtonValue.NO);
            response.nextState = negativeGameState;
        }
    }
}
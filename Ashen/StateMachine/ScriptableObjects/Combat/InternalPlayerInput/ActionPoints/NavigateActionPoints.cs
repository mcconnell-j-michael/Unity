using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class NavigateActionPoints : A_PlayerInputState
    {
        private CombatTool combatTool;
        private int currentIndex;
        private I_GameState parentState;

        public NavigateActionPoints(I_GameState parentState)
        {
            ToolManager tm = PlayerInputState.Instance.currentlySelected;
            combatTool = tm.Get<CombatTool>();
            currentIndex = 0;
            this.parentState = parentState;
        }

        protected override IEnumerator PreProcessState()
        {
            if (combatTool.Count() == 0)
            {
                response.nextState = parentState;
                yield break;
            }
            if (currentIndex >= combatTool.Count() || currentIndex < 0)
            {
                currentIndex = 0;
            }
            combatTool.Select(currentIndex);
            yield break;
        }

        public override void OnSelectLeft()
        {
            if (IsDelayed())
            {
                return;
            }
            SetSelectDelay(0.15f);
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = combatTool.Count() - 1;
            }
            combatTool.Select(currentIndex);
        }

        public override void OnSelectRight()
        {
            if (IsDelayed())
            {
                return;
            }
            SetSelectDelay(0.15f);
            currentIndex++;
            if (currentIndex >= combatTool.Count())
            {
                currentIndex = 0;
            }
            combatTool.Select(currentIndex);
        }

        public override void OnSubmit()
        {
            response.nextState = new NavigateConfirmationDialogue("Cancel action?", new CancelAction(
                this, currentIndex, combatTool),
                this
            );
        }

        public override void OnCancel()
        {
            combatTool.DeselectAll();
            response.nextState = parentState;
        }
    }
}
using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class CancelAction : I_GameState
    {
        private int actionIndex;
        private I_GameState returnToState;
        private CombatTool combatTool;

        public CancelAction(I_GameState returnToState, int actionIndex, CombatTool combatTool)
        {
            this.returnToState = returnToState;
            this.actionIndex = actionIndex;
            this.combatTool = combatTool;
        }

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            combatTool.Rollback(actionIndex);
            response.nextState = returnToState;
            yield break;
        }
    }
}
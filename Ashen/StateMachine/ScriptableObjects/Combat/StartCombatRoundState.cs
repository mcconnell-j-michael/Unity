using Ashen.PauseSystem;
using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class StartCombatRoundState : I_GameState
    {
        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;

            CombatPortraitManager cpm = CombatPortraitManager.Instance;
            cpm.HideSprite(false);
            foreach (PartyPosition position in playerParty.GetActivePositions())
            {
                CombatTool combatTool = playerParty.GetToolManager(position).Get<CombatTool>();
                combatTool.FinalizeAndClear();
            }
            response.nextState = ExecuteInputState.Instance;
            yield return null;
        }
    }
}
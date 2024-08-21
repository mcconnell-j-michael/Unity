using Ashen.PartySystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class EndCombatState : I_GameState
    {
        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            PlayerPartyHolder.Instance.partyManager.GetPartyUIManager().CleanUp();
            EnemyPartyHolder.Instance.enemyPartyManager.GetPartyUIManager().CleanUp();
            yield break;
        }
    }
}
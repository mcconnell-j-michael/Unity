using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class InitiatePauseScreenState : SingletonScriptableObject<InitiatePauseScreenState>, I_GameState
    {
        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            A_PartyManager partyManager = PlayerPartyHolder.Instance.partyManager;
            partyManager.Refresh();

            response.nextState = new ChoosePauseOptionState(partyManager.partyToolManager);

            while (response.nextState != null)
            {
                yield return response.nextState.RunState(request, response);
            }
        }
    }
}
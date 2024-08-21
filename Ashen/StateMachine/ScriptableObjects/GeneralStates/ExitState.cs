using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class ExitState : I_GameState
    {
        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            response.nextState = null;
            yield break;
        }
    }
}
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class ProceedWithCombatRoundState : I_GameState
    {
        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            PlayerInputState.Instance.RequestProceedWithCombat();
            yield break;
        }
    }
}
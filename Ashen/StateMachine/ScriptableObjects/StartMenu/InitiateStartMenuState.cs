using Ashen.SaturationSystem;
using Ashen.StateMachineSystem;
using System.Collections;

public class InitiateStartMenuState : SingletonScriptableObject<InitiateStartMenuState>, I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        Saturation sat = Saturations.Instance[0];
        response.nextState = new ChooseStartMenuOptionState(null);
        yield return response.nextState.RunState(request, response);
    }
}

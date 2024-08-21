using System.Collections;

public class MovePreviousTurn : I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerInputState.Instance.RequestMovePrevious();
        response.nextState = null;
        yield break;
    }
}

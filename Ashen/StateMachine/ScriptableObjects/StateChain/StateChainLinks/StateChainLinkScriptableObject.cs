using Sirenix.OdinInspector;
using System.Collections;

public abstract class StateChainLinkScriptableObject : SerializedScriptableObject, I_GameState
{
    public bool setNextState;
    [ShowIf(nameof(setNextState))]
    public I_GameState nextState;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        yield return InnerRunState(request, response);
        if (setNextState)
        {
            response.nextState = nextState;
        }
    }

    public abstract IEnumerator InnerRunState(GameStateRequest request, GameStateResponse response);
}

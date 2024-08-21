using Sirenix.OdinInspector;
using System.Collections;

public class GameStateManager : SerializedScriptableObject, I_GameState
{
    public I_GameState initialState;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        if (initialState == null)
        {
            throw new System.Exception("Initial state is null and cannot be run. From: " + this.name);
        }
        if (!request.runner)
        {
            throw new System.Exception("Cannot initialize " + this.name + " because the passed in runner is null or not active");
        }
        I_GameState nextState = initialState;
        I_GameState lastState = this;
        while (nextState != null)
        {
            GameStateResponse newResponse = new()
            {
                parent = response,
            };
            GameStateRequest newRequest = new()
            {
                lastState = lastState,
                runner = request.runner
            };
            yield return request.runner.StartCoroutine(nextState.RunState(newRequest, newResponse));
            lastState = nextState;
            nextState = newResponse.nextState;
        }
        response.completedState = initialState;
    }
}


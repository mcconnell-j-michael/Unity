using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChain : SerializedScriptableObject, I_GameState
{
    public List<I_GameState> states;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        if (states == null || states.Count < 1)
        {
            throw new System.Exception("Initial state is null and cannot be run. From: " + this.name);
        }
        if (!request.runner)
        {
            throw new System.Exception("Cannot initialize " + this.name + " because the passed in runner is null or not active");
        }

        I_GameState lastState = this;
        GameStateResponse newResponse = new GameStateResponse();
        foreach (I_GameState state in states)
        { 
            GameStateRequest newRequest = new GameStateRequest
            {
                lastState = lastState,
                runner = request.runner
            };
            yield return request.runner.StartCoroutine(state.RunState(newRequest, newResponse));
            lastState = state;
            response.completedState = state;
        }
        response.nextState = newResponse.nextState;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNextState : I_GameState
{
    public I_GameState nextState;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        response.nextState = nextState;
        yield break;
    }
}

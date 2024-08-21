using UnityEngine;
using System.Collections;

public interface I_GameState
{
    IEnumerator RunState(GameStateRequest request, GameStateResponse response);
}

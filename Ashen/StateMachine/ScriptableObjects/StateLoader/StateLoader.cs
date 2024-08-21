using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class StateLoader : SerializedScriptableObject, I_GameState
{
    [OdinSerialize]
    private Dictionary<LoadOption, I_GameState> gameStateMap;
    
    [NonSerialized]
    private LoadOption loadOptionSelected;
    private bool substate;

    public void ExecuteState(LoadOption loadOption, bool substate = false)
    {
        if (!gameStateMap.ContainsKey(loadOption))
        {
            Logger.ErrorLog("The state: " + loadOption.name + " was selected for: " + this.name + " but this state is not set.");
            return;
        }
        loadOptionSelected = loadOption;
        this.substate = substate;
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        loadOptionSelected = null;
        bool keepGoing = true;
        while (keepGoing)
        {
            if (loadOptionSelected)
            {
                if (substate)
                {
                    GameStateManager state = CreateInstance<GameStateManager>();
                    state.initialState = gameStateMap[loadOptionSelected];
                    GameStateResponse newGameStateResponse = new GameStateResponse();
                    yield return state.RunState(request, newGameStateResponse);
                }
                else
                {
                    response.nextState = gameStateMap[loadOptionSelected];
                }
                loadOptionSelected = null;
                keepGoing = substate;
            }
            else
            {
                yield return null;
            }
        }
    }
}

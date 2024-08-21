using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class GameStateRunner : MonoBehaviour
{
    public bool defaultStateEnabled;
    public GameStateManager defaultState;

    public void Start()
    {
        if (!defaultStateEnabled || !defaultState)
        {
            return;
        } 
        this.StartCoroutine(defaultState.RunState(
           new GameStateRequest
           {
               runner = this
           },
           new GameStateResponse()
      ));
    }

    [Button]
    public void InitializeState(GameStateManager manager)
    {
        this.StartCoroutine(manager.RunState(
            new GameStateRequest
            {
                runner = this
            },
            new GameStateResponse()
       ));
    }
}

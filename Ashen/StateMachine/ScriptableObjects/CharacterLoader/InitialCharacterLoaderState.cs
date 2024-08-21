using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialCharacterLoaderState : SingletonScriptableObject<InitialCharacterLoaderState>, I_GameState
{
    public string uiSceneName;
    public I_GameState nextState;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            Debug.Log("Loading ui Scene: " + uiSceneName + " Progress: " + operation.progress);
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByName(uiSceneName);

        yield return null;

        PlayerPartyManager manager = FindObjectOfType<PlayerPartyManager>();

        SceneManager.MoveGameObjectToScene(manager.gameObject, SceneManager.GetSceneByName("SceneManager"));

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(uiSceneName);

        while (!unloadOperation.isDone)
        {
            Debug.Log("Unloading ui Scene: " + uiSceneName + " Progress: " + unloadOperation.progress);
            yield return null;
        }

        response.nextState = nextState;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : I_GameState
{
    public SceneLabel sceneLabel;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLabel.sceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            Debug.Log("Loading ui Scene: " + sceneLabel.sceneName + " Progress: " + operation.progress);
            yield return null;
        }

        yield break;
    }
}

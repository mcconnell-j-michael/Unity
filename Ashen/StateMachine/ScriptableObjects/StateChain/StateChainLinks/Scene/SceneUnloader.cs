using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : I_GameState
{
    public SceneLabel sceneLabel;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneLabel.sceneName);

        while (!unloadOperation.isDone)
        {
            Debug.Log("Unloading ui Scene: " + sceneLabel.sceneName + " Progress: " + unloadOperation.progress);
            yield return null;
        }

        yield break;
    }
}

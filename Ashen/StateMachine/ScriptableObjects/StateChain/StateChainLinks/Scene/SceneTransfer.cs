using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransfer : StateChainLinkScriptableObject
{
    public SceneLabel toScene;

    public override IEnumerator InnerRunState(GameStateRequest request, GameStateResponse response)
    {
        TransferObject[] objects = FindObjectsOfType<TransferObject>();
        Scene scene = SceneManager.GetSceneByName(toScene.sceneName);
        if (objects != null)
        {
            foreach (TransferObject transferObj in objects)
            {
                SceneManager.MoveGameObjectToScene(transferObj.gameObject, scene);
                if (transferObj.temporary)
                {
                    Destroy(transferObj);
                }
            }
        }
        yield break;
    }
}

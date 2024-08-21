using UnityEngine;
using UnityEditor;

namespace Ashen.GameManagerWindow
{
    [CreateAssetMenu(fileName = "DefaultGameManager", menuName = "Custom/GameManager/DefaultGameManager")]
    public class DefaultGameManagerConfiguration : SingletonScriptableObject<DefaultGameManagerConfiguration>
    {
        public GameManagerStateCollection gameManagerStateCollection;

        [MenuItem("Tools/Game Manager")]
        public static void OpenWindow()
        {
            GameManager gameManager = EditorWindow.GetWindow<GameManager>(typeof(GameManager));
            gameManager.Initialize(Instance.gameManagerStateCollection, "Game Manager", "Used to edit general scriptable objects");
            gameManager.Show();
        }
    }
}
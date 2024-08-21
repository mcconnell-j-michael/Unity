using UnityEngine;
using UnityEditor;

namespace Ashen.GameManagerWindow
{
    public class DrawSubManager
    {
        [HideInInspector]
        public GameManager gameManager;
        private GameManagerStateCollection collection;
        private string title;
        private string subtitle;

        public DrawSubManager(GameManagerStateCollection collection, string title, string subtitle)
        {
            this.collection = collection;
            this.title = title;
            this.subtitle = subtitle;
        }

        public void OnSelected()
        {
            gameManager = EditorWindow.CreateWindow<GameManager>(title, typeof(GameManager));
            gameManager.Initialize(collection, title, subtitle);
            gameManager.Show();
        }

        public void OnDeselected()
        {
            gameManager.Close();
            gameManager = null;
        }
    }
}
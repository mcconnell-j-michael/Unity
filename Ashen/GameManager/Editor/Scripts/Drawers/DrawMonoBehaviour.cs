using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Ashen.GameManagerWindow
{
    public class DrawMonoBehaviour
    {
        [ShowIf("@monoBehaviour != null")]
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public MonoBehaviour monoBehaviour;
        
        private GameManagerMonoBehaviourOption option;

        public DrawMonoBehaviour(GameManagerMonoBehaviourOption option)
        {
            this.option = option;
            FindMyObject();
        }

        public void FindMyObject()
        {
            if (!monoBehaviour)
            {
                monoBehaviour = GameObject.FindObjectOfType(option.sourceType.Type) as MonoBehaviour;
            }
        }

        [ShowIf("@monoBehaviour != null")]
        [GUIColor(0.7f,1f,0.7f)]
        [ButtonGroup("Top Button", -1000)]
        private void SelectSceneObject()
        {
            if (!monoBehaviour)
            {
                Selection.activeGameObject = monoBehaviour.gameObject;
            }
        }

        [ShowIf("@monoBehaviour == null")]
        [Button]
        private void CreateManagerObject()
        {
            GameObject newManager = new GameObject();
            newManager.name = "New " + option.sourceType.Type.ToString();
            newManager.AddComponent(option.sourceType.Type);
        }
    }
}
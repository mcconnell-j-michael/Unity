using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System;

namespace Ashen.GameManagerWindow
{
    //Used to draw the current object that is selected in the Menu Tree
    //Look at me using generics ;)
    public class DrawScriptableObject
    {
        //[Title("@property.name")]
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public ScriptableObject selected;

        [LabelWidth(100)]
        [PropertyOrder(-1)]
        [BoxGroup("CreateNew")]
        [HorizontalGroup("CreateNew/Horizontal")]
        public string nameForNew;

        private GameManagerAssetOption gameManagerOption;

        public DrawScriptableObject(GameManagerAssetOption gameManagerOption)
        {
            this.gameManagerOption = gameManagerOption;
        }

        [HorizontalGroup("CreateNew/Horizontal")]
        [GUIColor(0.7f, 0.7f, 1f)]
        [Button]
        public void CreateNew()
        {
            if (nameForNew == "" || nameForNew == null)
                return;

            ScriptableObject newItem = ScriptableObject.CreateInstance(gameManagerOption.sourceType.Type);
            newItem.name = nameForNew;
            string path = gameManagerOption.DefaultCreatePath;
            if (path == null)
            {
                gameManagerOption.OnSourcePathChanged();
            }
            if (selected != null)
            {
                path = AssetDatabase.GetAssetPath(selected);
                path = path.Substring(0, path.LastIndexOf('/'));
            }
            AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
            AssetDatabase.SaveAssets();

            gameManagerOption.OnNew(newItem);

            nameForNew = "";
        }

        [HorizontalGroup("CreateNew/Horizontal")]
        [GUIColor(1f, 0.7f, 0.7f)]
        [Button]
        public void DeleteSelected()
        {
            if (selected != null)
            {
                string path = AssetDatabase.GetAssetPath(selected);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                gameManagerOption.OnDelete(selected);
            }
        }

        public void SetSelected(object item)
        {
            //ensure selection is of the correct type
            ScriptableObject attempt = item as ScriptableObject;
            if (attempt != null)
                selected = attempt;
        }
    }
}
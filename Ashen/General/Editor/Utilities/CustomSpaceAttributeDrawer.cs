using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EditorUtilities
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    public class CustomSpaceAttributeDrawer : OdinAttributeDrawer<CustomSpaceAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (this.Attribute.spaceBefore)
            {
                EditorGUILayout.Space();
            }
            CallNextDrawer(label);
            if (this.Attribute.spaceAfter)
            {
                EditorGUILayout.Space();
            }
        }
    }
}
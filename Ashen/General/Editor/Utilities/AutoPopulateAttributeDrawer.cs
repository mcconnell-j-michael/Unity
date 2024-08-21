using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector.Editor;
using System;
using System.Reflection;

[DrawerPriority(DrawerPriorityLevel.SuperPriority)]
public class AutoPopulateAttributeDrawer<T> : OdinAttributeDrawer<AutoPopulate, T>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (this.ValueEntry.SmartValue == null)
        {
            Type type = this.Attribute.instance;
            if (type == null)
            {
                type = typeof(T);
            }
            if (!typeof(ScriptableObject).IsAssignableFrom(type) && !type.IsValueType)
            {
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    this.ValueEntry.SmartValue = (T) constructor.Invoke(new object[0]);
                }
            }
            
        }
        this.CallNextDrawer(label);
    }
}

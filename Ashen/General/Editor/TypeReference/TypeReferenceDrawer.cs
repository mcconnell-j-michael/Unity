using UnityEngine;
using Sirenix.OdinInspector.Editor;
using System;
using Sirenix.Utilities;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;

public class TypeReferenceDrawer : OdinValueDrawer<TypeReference>
{
    private static HashSet<Type> types = new HashSet<Type>();
    private static Type currentSelectedType;

    private Func<Rect, OdinSelector<Type>> createTypeSelector = (rect) =>
    {
        TypeSelector selector = new TypeSelector(types, false);
        selector.SetSelection(currentSelectedType);
        selector.ShowInPopup(rect);
        return selector;
    };

    public TypeReferenceDrawer() : base()
    {
        types = null;
    }

    private static HashSet<Type> GetTypes(ClassTypeContraintAttribute classTypeContraintAttribute)
    {
        HashSet<Type> types = new HashSet<Type>();
        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in a.GetTypes())
            {
                if (!type.IsVisible)
                {
                    continue;
                }
                if (classTypeContraintAttribute == null || classTypeContraintAttribute.IsConstrainedType(type))
                {
                    types.Add(type);
                }
            }
        }
        return types;
    }

    protected override void Initialize()
    {
        if (this.ValueEntry.SmartValue == null)
        {
            this.ValueEntry.SmartValue = new TypeReference();
        }
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        if (this.ValueEntry.SmartValue == null)
        {
            this.ValueEntry.SmartValue = new TypeReference();
        }
        if (types == null)
        {
            InspectorProperty property = this.Property;
            ClassTypeContraintAttribute constrainedAttribute = property.GetAttribute<ClassTypeContraintAttribute>();
            types = GetTypes(constrainedAttribute);
        }
        TypeReference typeReference = this.ValueEntry.SmartValue;
        Type currentType = typeReference.Type;
        currentSelectedType = currentType;
        string dropdownText = currentType == null ? "None" : currentType.GetNiceName();
        IEnumerable<Type> selected = TypeSelector.DrawSelectorDropdown(label, dropdownText, createTypeSelector);
        if (selected != null && selected.Any())
        {
            currentType = selected.FirstOrDefault();
        }
        typeReference.Type = currentType;
        //base.CallNextDrawer(label);
    }
}
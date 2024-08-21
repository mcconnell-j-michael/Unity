using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectCreatorUtilities
{
    public static HashSet<Type> GetTypeHashSet()
    {
        return Sirenix.Utilities.AssemblyUtilities.GetTypes(Sirenix.Utilities.AssemblyCategory.ProjectSpecific)
        .Where(t =>
            t.IsClass &&
            typeof(ScriptableObject).IsAssignableFrom(t) &&
            !typeof(EditorWindow).IsAssignableFrom(t) &&
            !typeof(Editor).IsAssignableFrom(t))
       .ToHashSet();
    }
}

using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
[HideLabel]
public class TypeReference : ISerializationCallbackReceiver
{
    [SerializeField]
    private string classRef;
    private Type type;

    public Type Type
    {
        get { return type; }
        set
        {
            if (value != null && !value.IsClass)
            {
                throw new ArgumentException(string.Format("'{0}' is not a class type.", value.FullName), "value");
            }

            if ((type != value || classRef == null) && value != null)
            {
                type = value;
                classRef = GetClassRef(value);
            }
        }
    }

    public TypeReference() { }

    public TypeReference(string assemblyQualifiedClassName)
    {
        Type = !string.IsNullOrEmpty(assemblyQualifiedClassName)
            ? Type.GetType(assemblyQualifiedClassName)
            : null;
    }

    public static string GetClassRef(Type type)
    {
        return type != null
            ? type.FullName + ", " + type.Assembly.GetName().Name
            : "";
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        if (!string.IsNullOrEmpty(classRef))
        {
            type = System.Type.GetType(classRef);

            if (type == null)
            {
                Debug.LogWarning(string.Format("'{0}' was referenced but class type was not found.", classRef));
            }
        }
        else
        {
            type = null;
        }
    }
}

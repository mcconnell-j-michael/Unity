using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ClassTypeContraintAttribute : PropertyAttribute
{
    private Type type;

    private bool allowAbstract = true;
    public bool AllowAbstract
    {
        get { return allowAbstract; }
        set { allowAbstract = value; }
    }
    private bool allowInterface = true;
    public bool AllowInterface
    {
        get { return allowInterface; }
        set { allowInterface = value; }
    }

    public ClassTypeContraintAttribute(Type type)
    {
        this.type = type;
    }

    public bool IsConstrainedType(Type type)
    {
        if (!AllowAbstract && type.IsAbstract)
        {
            return false;
        }
        if (!AllowInterface && type.IsInterface)
        {
            return false;
        }
        if (this.type.IsGenericType)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == this.type)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }
        return this.type.IsAssignableFrom(type);
    }
}

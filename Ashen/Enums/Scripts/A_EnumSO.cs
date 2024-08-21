using Ashen.EnumSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

/**
 * The EnumSO allows for the creation of ScriptableObjects as Enums
 **/
[Serializable]
public abstract class A_EnumSO<T, E> : SerializedScriptableObject, I_EnumSO where T : A_EnumSO<T, E> where E : A_EnumList<T, E>
{
    [OdinSerialize]
    protected int index;

    [NonSerialized]
    private bool toDestroy = false;
    public bool ToDestroy
    {
        get
        {
            return toDestroy;
        }
    }

    [NonSerialized]
    private bool ensured = false;

    public int Index
    {
        get
        {
            if (!ensured)
            {
                EnsureEnumeration();
            }
            return index;
        }
        set
        {
            index = value;
        }
    }

    public List<T> GetListLocal()
    {
        return A_EnumList<T, E>.EnumList;
    }

    public static List<T> GetList()
    {
        return A_EnumList<T, E>.EnumList;
    }

    [Button]
    public void EnsureEnumeration()
    {
        if (!TimeManager.isLoading && A_EnumList<T, E>.Instance && !A_EnumList<T, E>.Instance.disableAutoChecks)
        {
            if (A_EnumList<T, E>.Instance[index] != this)
            {
                A_EnumList<T, E>.Instance.Recount();
                if (A_EnumList<T, E>.Instance[index] != this)
                {
                    A_EnumList<T, E>.Instance.Add((T)this);
                }
            }
            ensured = true;
        }
    }

    public void OnDestroy()
    {
        toDestroy = true;
        if (A_EnumList<T, E>.Instance)
        {
            A_EnumList<T, E>.Instance.Recount();
        }
    }

    public void OnEnable()
    {
        EnsureEnumeration();
    }

    public override string ToString()
    {
        return name;
    }

    public int GetIndex()
    {
        return index;
    }

    public static explicit operator int(A_EnumSO<T, E> v)
    {
        if (v is null)
        {
            return -1;
        }
        return v.Index;
    }
}

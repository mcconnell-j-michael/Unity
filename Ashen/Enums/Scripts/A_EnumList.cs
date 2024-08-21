using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * The EnumList holds all the Enums of a specifict type. It also will enumerate all the EnumSO's in the list.
 **/
public abstract class A_EnumList<T, E> : SingletonScriptableObject<E>, IEnumerable<T> where T : A_EnumSO<T, E> where E : A_EnumList<T, E>
{
    [OdinSerialize, AutoPopulate, HideIf("@" + nameof(HideList) + "()")]
    protected List<T> enumList = default;

    [NonSerialized]
    public bool disableAutoChecks = false;

    protected virtual List<T> GetEnumList()
    {
        disableAutoChecks = false;
        return enumList;
    }

    protected virtual bool HideList()
    {
        return false;
    }

    [NonSerialized]
    private Dictionary<string, T> enumMap;
    private Dictionary<string, T> EnumMap
    {
        get
        {
            if (enumMap == null)
            {
                enumMap = new Dictionary<string, T>();
                foreach (T enumSo in GetEnumList())
                {
                    enumMap.Add(enumSo.name, enumSo);
                }
            }
            return enumMap;
        }
    }

    public static T GetEnum(string enumName)
    {
        if (Instance.EnumMap.TryGetValue(enumName, out T enumSo))
        {
            return enumSo;
        }
        throw new Exception("Could not find enum: " + enumName + " from list: " + Instance.name);
    }

    public static T GetEnum(int enumValue)
    {
        return Instance[enumValue];
    }

    public static List<T> EnumList
    {
        get
        {
            return Instance.GetEnumList();
        }
    }

    public static int Count
    {
        get
        {
            if (Instance.GetEnumList() == null)
            {
                Logger.ErrorLog("enumList in EnumList cannot be null");
                return 0;
            }
            return Instance.GetEnumList().Count;
        }
    }

    public T this[int i]
    {
        get
        {
            if (GetEnumList() == null || i < 0 || i >= GetEnumList().Count)
            {
                return null;
            }
            return GetEnumList()[i];
        }
    }

    private void OnEnable()
    {
        Recount();
    }

    [Button]
    public void Recount()
    {
        if (disableAutoChecks)
        {
            return;
        }
        if (GetEnumList() != null)
        {
            for (int x = 0; x < GetEnumList().Count; x++)
            {
                if (GetEnumList()[x] == null || GetEnumList()[x].ToDestroy)
                {
                    GetEnumList().RemoveAt(x);
                    x--;
                }
                else
                {
                    GetEnumList()[x].Index = x;
                }

            }
        }
    }

    public void Add(T t)
    {
        GetEnumList().Add(t);
        Recount();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int x = 0; x < GetEnumList().Count; x++)
        {
            yield return GetEnumList()[x];
        }
    }
}

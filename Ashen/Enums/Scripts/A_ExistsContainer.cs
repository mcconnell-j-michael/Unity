using System;
using System.Runtime.Serialization;

/**
 * An ExistsContainer allows for checking if an EnumSO exists within its list. This is useful for things like DamageContainers, 
 * so you can check if a certain DamageType is MagicalDamage or not (i.e Fire would be a typical magic damage type, while Piercing would not be)
 **/
[Serializable]
public class A_ExistsContainer<T, E> : A_EnumContainer<T, E> where T : A_EnumSO<T, E> where E : A_EnumList<T, E>
{
    [NonSerialized]
    private bool[] enumBools;
    [NonSerialized]
    private int trueCount;

    private void Initialize()
    {
        enumBools = new bool[A_EnumList<T,E>.EnumList.Count];
        trueCount = 0;
        foreach (T t in GetEnums())
        {
            enumBools[t.Index] = true;
            trueCount++;
        }
    }
    
    private void EnsureContainer()
    {
        if (enumBools == null)
        {
            Initialize();
        }
        else
        {
            if (enumBools.Length != A_EnumList<T,E>.EnumList.Count || trueCount != GetEnums().Count)
            {
                Initialize();
            }
        }
    }

    public bool IsType(T enumSO)
    {
        EnsureContainer();
        return enumBools[(int)enumSO];
    }

    public bool IsType(int index)
    {
        EnsureContainer();
        if (index > enumBools.Length || index < 0)
        {
            return false;
        }
        return enumBools[index];
    }

    public A_ExistsContainer(SerializationInfo info, StreamingContext context) : base(info, context){}
}

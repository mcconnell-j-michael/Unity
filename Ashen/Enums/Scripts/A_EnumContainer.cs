using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using Sirenix.Serialization;

/**
 * The EnumContainer allows for grouping certian EnumSOs together
 **/
[Serializable]
public abstract class A_EnumContainer<T, E> : ISerializable where T : A_EnumSO<T, E> where E : A_EnumList<T, E>
{
    [AutoPopulate, EnumSODropdown, OdinSerialize]
    private List<T> enums;

    private List<int> enumNums;

    public A_EnumContainer(SerializationInfo info, StreamingContext context)
    {
        enumNums = (List<int>)info.GetValue(nameof(enums), typeof(List<int>));
    }

    public List<T> GetEnums()
    {
        if (enums == null)
        {
            enums = new List<T>();
            foreach (int enumNum in enumNums)
            {
                T enumValue = A_EnumList<T, E>.GetEnum(enumNum);
                if (enumValue)
                {
                    enums.Add(enumValue);
                }
            }
        }
        return enums;
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        List<int> enumNums = new List<int>();
        if (enums != null)
        {
            foreach (T e in enums)
            {
                enumNums.Add((int)e);
            }
        }
        info.AddValue(nameof(enums), enumNums);
    }
}
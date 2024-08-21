using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EnumSystem
{
    public abstract class A_DependentEnumList<DependentEnumSO, DependentEnumList, DependsOnEnumOne, DependsOnEnumListOne, DependsOnEnumTwo, DependsOnEnumListTwo> :
        A_EnumList<DependentEnumSO, DependentEnumList>
        where DependentEnumSO : A_DependentEnumSO<DependentEnumSO, DependentEnumList, DependsOnEnumOne, DependsOnEnumListOne, DependsOnEnumTwo, DependsOnEnumListTwo>
        where DependentEnumList : A_DependentEnumList<DependentEnumSO, DependentEnumList, DependsOnEnumOne, DependsOnEnumListOne, DependsOnEnumTwo, DependsOnEnumListTwo>
        where DependsOnEnumOne : A_EnumSO<DependsOnEnumOne, DependsOnEnumListOne>
        where DependsOnEnumListOne : A_EnumList<DependsOnEnumOne, DependsOnEnumListOne>
        where DependsOnEnumTwo : A_EnumSO<DependsOnEnumTwo, DependsOnEnumListTwo>
        where DependsOnEnumListTwo : A_EnumList<DependsOnEnumTwo, DependsOnEnumListTwo>
    {
        [NonSerialized]
        private bool initialized = false;
        [ShowInInspector, NonSerialized, HideInEditorMode]
        private List<DependentEnumSO> internalEnumList = default;

        protected override List<DependentEnumSO> GetEnumList()
        {
            if (!Application.isPlaying)
            {
                return enumList;
            }
            if (initialized)
            {
                return internalEnumList;
            }
            disableAutoChecks = true;
            initialized = true;
            internalEnumList = new List<DependentEnumSO>();
            if (enumList != null)
            {
                internalEnumList.AddRange(enumList);
            }
            int total = A_EnumList<DependsOnEnumOne, DependsOnEnumListOne>.Count * A_EnumList<DependsOnEnumTwo, DependsOnEnumListTwo>.Count;
            List<DependentEnumSO> spareEnums = new();
            while (internalEnumList.Count < total)
            {
                internalEnumList.Add(null);
            }
            for (int x = 0; x < internalEnumList.Count; x++)
            {
                DependentEnumSO dependentEnum = internalEnumList[x];
                if (dependentEnum)
                {
                    if (!dependentEnum.firstDependency || !dependentEnum.secondDependency)
                    {
                        spareEnums.Add(dependentEnum);
                        internalEnumList[x] = null;
                        continue;
                    }
                    int trueIndex = GetIndex(dependentEnum.firstDependency, dependentEnum.secondDependency);
                    if (x != trueIndex)
                    {
                        DependentEnumSO swapEnum = internalEnumList[trueIndex];
                        if (!swapEnum)
                        {
                            internalEnumList[trueIndex] = dependentEnum;
                            internalEnumList[x] = null;
                            continue;
                        }
                        if ((!swapEnum.firstDependency || !swapEnum.secondDependency) || (swapEnum.firstDependency == dependentEnum.firstDependency && swapEnum.secondDependency == dependentEnum.secondDependency))
                        {
                            spareEnums.Add(swapEnum);
                            internalEnumList[trueIndex] = dependentEnum;
                            internalEnumList[x] = null;
                            continue;
                        }
                        internalEnumList[trueIndex] = dependentEnum;
                        internalEnumList[x] = swapEnum;
                        x--;
                        continue;
                    }
                }
            }
            int totalSeondDependency = A_EnumList<DependsOnEnumTwo, DependsOnEnumListTwo>.Count;
            int totalFirstDependency = A_EnumList<DependsOnEnumOne, DependsOnEnumListOne>.Count;
            for (int x = 0; x < totalFirstDependency; x++)
            {
                for (int y = 0; y < totalSeondDependency; y++)
                {
                    int index = (totalSeondDependency * x) + y;
                    if (internalEnumList[index])
                    {
                        continue;
                    }
                    DependentEnumSO dependentEnum = null;
                    if (spareEnums.Count > 0)
                    {
                        dependentEnum = spareEnums[0];
                        spareEnums.RemoveAt(0);
                    }
                    else
                    {
                        dependentEnum = CreateInstance<DependentEnumSO>();
                    }
                    dependentEnum.firstDependency = A_EnumList<DependsOnEnumOne, DependsOnEnumListOne>.Instance[x];
                    dependentEnum.secondDependency = A_EnumList<DependsOnEnumTwo, DependsOnEnumListTwo>.Instance[y];
                    dependentEnum.Index = index;
                    dependentEnum.name = dependentEnum.firstDependency.name + "_" + dependentEnum.secondDependency.name;
                    internalEnumList[index] = dependentEnum;
                }
            }
            disableAutoChecks = false;
            GetEnumListCleanup();
            return internalEnumList;
        }

        protected int GetIndex(DependsOnEnumOne one, DependsOnEnumTwo two)
        {
            return (((int)one) * A_EnumList<DependsOnEnumTwo, DependsOnEnumListTwo>.Count) + ((int)two);
        }

        protected virtual void GetEnumListCleanup() { }

        protected override bool HideList()
        {
            return Application.isPlaying;
        }
    }
}
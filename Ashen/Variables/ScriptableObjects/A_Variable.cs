using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.VariableSystem
{
    public abstract class A_Variable<T> : SerializedScriptableObject
    {
        public T value;
    }
}
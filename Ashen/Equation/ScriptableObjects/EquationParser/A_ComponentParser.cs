using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public abstract class A_ComponentParser<T> : SerializedScriptableObject, I_ComponentParser
    {
        public Dictionary<string, T> parseMap;

        public bool StringValid(string toParse)
        {
            if (parseMap != null && parseMap.ContainsKey(toParse))
            {
                return true;
            }
            return false;
        }

        public abstract I_EquationComponent GetEquationComponent(string toParse);
    }
}
using UnityEngine;
using System.Collections;

namespace Ashen.EquationSystem
{
    public interface I_ComponentParser
    {
        bool StringValid(string toParse);
        I_EquationComponent GetEquationComponent(string toParse);
    }
}
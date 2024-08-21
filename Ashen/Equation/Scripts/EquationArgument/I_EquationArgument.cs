using UnityEngine;
using System.Collections;

namespace Ashen.EquationSystem
{
    public interface I_EquationArgument
    {
        float GetValue();
        int DefaultValue();
        string GetKey();
    }
}
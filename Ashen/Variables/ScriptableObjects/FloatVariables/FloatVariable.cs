using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.VariableSystem
{
    [CreateAssetMenu(fileName = nameof(FloatVariable), menuName = "Custom/Variables/Float")]
    public class FloatVariable : A_Variable<float>
    { }
}
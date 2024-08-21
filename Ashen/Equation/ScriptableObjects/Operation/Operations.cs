using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(Operations), menuName = "Custom/Enums/" + nameof(Operations) + "/Types")]
    public class Operations : A_EnumList<A_Operation, Operations>
    {
        public A_Operation MULTIPLY;
        public A_Operation DIVIDE;
        public A_Operation OPENPARAM;
        public A_Operation CLOSEPARAM;
        public A_Operation ADD;
        public A_Operation SUBTRACT;
        public A_Operation ARGUMENT_SEPARATOR;
    }
}
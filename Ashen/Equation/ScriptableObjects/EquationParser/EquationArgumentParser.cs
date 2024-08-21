using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(EquationArgumentParser), menuName = "Custom/EquationParser/" + nameof(EquationArgumentParser))]
    public class EquationArgumentParser : A_ComponentParser<GeneralEquationArgument>
    {
        public override I_EquationComponent GetEquationComponent(string toParse)
        {
            if (StringValid(toParse))
            {
                return parseMap[toParse];
            }
            return null;
        }
    }
}
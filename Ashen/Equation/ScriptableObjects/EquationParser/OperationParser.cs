using UnityEngine;
using System.Collections;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(OperationParser), menuName = "Custom/EquationParser/" + nameof(OperationParser))]
    public class OperationParser : A_ComponentParser<A_Operation>
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
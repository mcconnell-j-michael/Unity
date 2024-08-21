using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(AttributeParser), menuName = "Custom/EquationParser/" + nameof(AttributeParser))]
    public class AttributeParser : A_ComponentParser<DerivedAttribute>
    {
        public override I_EquationComponent GetEquationComponent(string toParse)
        {
            List<string> arguments = new();
            if (toParse.Contains("_"))
            {
                string key = toParse.Substring(0, toParse.IndexOf('_'));
                if (toParse.IndexOf("_") < toParse.Length - 1)
                {
                    string argumentString = toParse.Substring(toParse.IndexOf('_') + 1);
                    foreach (char character in argumentString)
                    {
                        arguments.Add((character + "").ToLower());
                    }
                }
                toParse = key;
            }
            if (StringValid(toParse))
            {
                AttributeValue attributeValue = new AttributeValue
                {
                    enumSO = parseMap[toParse]
                };
                if (arguments.Contains("t"))
                {
                    attributeValue.useTarget = true;
                }
                return attributeValue;
            }
            return null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.EquationSystem
{
    public abstract class A_DefaultComponentParser : I_ComponentParser
    {
        protected abstract List<string> GetValidArguments();

        public I_EquationComponent GetEquationComponent(string toParse)
        {
            List<string> arguments = new();
            List<string> validArguments = GetValidArguments();
            if (toParse.Contains("_"))
            {
                string key = toParse.Substring(0, toParse.IndexOf('_'));
                if (toParse.IndexOf("_") < toParse.Length - 1)
                {
                    string argumentString = toParse.Substring(toParse.IndexOf('_') + 1);
                    if (!validArguments.Contains(argumentString))
                    {
                        return null;
                    }
                    foreach (char character in argumentString)
                    {
                        arguments.Add((character + "").ToLower());
                    }
                }
                toParse = key;
            }

            return GetEquationComponentInternal(toParse, arguments);
        }

        protected abstract I_EquationComponent GetEquationComponentInternal(string toParse, List<string> arguments);

        public bool StringValid(string toParse)
        {
            List<string> validArguments = GetValidArguments();
            if (toParse.Contains("_"))
            {
                string key = toParse.Substring(0, toParse.IndexOf('_'));
                if (toParse.IndexOf("_") < toParse.Length - 1)
                {
                    string argumentString = toParse.Substring(toParse.IndexOf('_') + 1);
                    if (!validArguments.Contains(argumentString))
                    {
                        return false;
                    }
                }
                toParse = key;
            }
            return StringValidInternal(toParse);
        }

        protected abstract bool StringValidInternal(string toParse);
    }
}
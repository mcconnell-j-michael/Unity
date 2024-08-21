using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public class DefaultResourceValueParser : A_DefaultComponentParser
    {
        protected override I_EquationComponent GetEquationComponentInternal(string toParse, List<string> arguments)
        {
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                if (resourceValue.name.ToLower().Equals(toParse.ToLower()))
                {
                    ResourceValueValue resourceValueValue = new ResourceValueValue
                    {
                        enumSO = resourceValue
                    };
                    if (arguments.Contains("t"))
                    {
                        resourceValueValue.useTarget = true;
                    }
                    return resourceValueValue;
                }
            }

            return null;
        }

        private List<string> validArguments = new() { "t" };
        protected override List<string> GetValidArguments()
        {
            return validArguments;
        }

        protected override bool StringValidInternal(string toParse)
        {
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                if (resourceValue.name.ToLower().Equals(toParse.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
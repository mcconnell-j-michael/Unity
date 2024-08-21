using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public class DefaultTierLevelParser : A_DefaultComponentParser
    {
        protected override I_EquationComponent GetEquationComponentInternal(string toParse, List<string> arguments)
        {
            if (!toParse.Equals("tierlevel"))
            {
                return null;
            }
            TierLevelContainerValue tierLevelContainerValue = new();
            if (arguments.Contains("t"))
            {
                tierLevelContainerValue.useTarget = true;
            }
            return tierLevelContainerValue;
        }

        private readonly List<string> validArguments = new() { "t" };
        protected override List<string> GetValidArguments()
        {
            return validArguments;
        }

        protected override bool StringValidInternal(string toParse)
        {
            return toParse.ToLower().Equals("tierlevel");
        }
    }
}
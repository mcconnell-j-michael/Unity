using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public class DefaultAttributeParser : A_DefaultComponentParser
    {
        protected override I_EquationComponent GetEquationComponentInternal(string toParse, List<string> arguments)
        {
            foreach (DerivedAttribute attribute in DerivedAttributes.Instance)
            {
                if (attribute.name.ToLower().Equals(toParse.ToLower()))
                {
                    AttributeValue attributeValue = new AttributeValue
                    {
                        enumSO = attribute
                    };
                    if (arguments.Contains("t"))
                    {
                        attributeValue.useTarget = true;
                    }
                    return attributeValue;
                }
            }

            foreach (BaseAttribute baseAttribute in BaseAttributes.Instance)
            {
                if (baseAttribute.name.ToLower().Equals(toParse.ToLower()))
                {
                    BaseAttributeValue attributeValue = new BaseAttributeValue
                    {
                        enumSO = baseAttribute
                    };
                    if (arguments.Contains("t"))
                    {
                        attributeValue.useTarget = true;
                    }
                    return attributeValue;
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
            foreach (DerivedAttribute attribute in DerivedAttributes.Instance)
            {
                if (attribute.name.ToLower().Equals(toParse.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
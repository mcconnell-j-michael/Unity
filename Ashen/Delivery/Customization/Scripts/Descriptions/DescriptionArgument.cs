using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    public class DescriptionArgument
    {
        public string argument;
        public DescriptionArgumentType descriptionArgumentType;
        public I_Equation equation;

        public string GetString(I_DeliveryTool toolManager, DeliveryArgumentPacks extraArguments)
        {
            if (argument == null || descriptionArgumentType == null)
            {
                return "!NPE";
            }
            if (descriptionArgumentType == DescriptionArgumentTypes.Instance.STRING)
            {
                return argument;
            }
            if (descriptionArgumentType == DescriptionArgumentTypes.Instance.EQUATION_AVERAGE)
            {
                if (equation == null)
                {
                    return "!NPE";
                }
                return equation.Calculate(toolManager, extraArguments).ToString();
            }
            if (descriptionArgumentType == DescriptionArgumentTypes.Instance.EQUATION_RANGE)
            {
                if (equation == null)
                {
                    return "!NPE";
                }
                return equation.GetLow(toolManager, null, extraArguments) + "-" + equation.GetHigh(toolManager, null, extraArguments);
            }
            if (descriptionArgumentType == DescriptionArgumentTypes.Instance.EQUATION_DEFINITION)
            {
                if (equation == null)
                {
                    return "!NPE";
                }
                return equation.ToString();
            }
            return base.ToString();
        }
    }
}
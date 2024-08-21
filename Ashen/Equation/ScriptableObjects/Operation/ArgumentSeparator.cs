using Ashen.DeliverySystem;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = "_,", menuName = "Custom/Enums/Operations/Comma")]
    public class ArgumentSeparator : A_Operation
    {
        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            equation.keepGoing = false;
            return total;
        }

        public override string Representation()
        {
            return ",";
        }
    }
}
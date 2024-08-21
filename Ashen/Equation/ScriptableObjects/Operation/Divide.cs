using Ashen.DeliverySystem;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = "_∕", menuName = "Custom/Enums/Operations/" + nameof(Divide))]
    public class Divide : A_Operation
    {
        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            equation.currentIndex++;
            I_EquationComponent component = equation.equationComponents[equation.currentIndex];
            return total / component.Calculate(equation, source, target, total, extraArguments);
        }

        public override string Representation()
        {
            return "/";
        }
    }
}
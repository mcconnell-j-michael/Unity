using Ashen.DeliverySystem;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(Add), menuName = "Custom/Enums/Operations/" + nameof(Add))]
    public class Add : A_Operation
    {
        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            equation.currentIndex++;
            I_EquationComponent component = equation.equationComponents[equation.currentIndex];
            return component.Calculate(equation, source, target, total, extraArguments) + total;
        }

        public override string Representation()
        {
            return "+";
        }
    }
}
using Ashen.DeliverySystem;
using UnityEngine;

namespace Ashen.EquationSystem
{
    [CreateAssetMenu(fileName = nameof(CloseParam), menuName = "Custom/Enums/Operations/" + nameof(CloseParam))]
    public class CloseParam : A_Operation
    {
        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            equation.keepGoing = false;
            return total;
        }

        public override string Representation()
        {
            return ")";
        }
    }
}
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public abstract class A_ArgumentOperation : A_Operation
    {
        public int minimumArguments;
        public int maximumArguments;

        public abstract float RunOperation(List<float> args);

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            List<float> totals = new List<float>();
            equation.currentIndex++;
            while (equation.keepGoing)
            {
                equation.currentIndex++;
                I_EquationComponent component = equation.equationComponents[equation.currentIndex];
                totals.Add(equation.Calculate(source, target, equation.currentIndex, extraArguments));
                if (equation.equationComponents[equation.currentIndex] == (I_EquationComponent)Operations.Instance.ARGUMENT_SEPARATOR)
                {
                    equation.keepGoing = true;
                }
            }
            equation.keepGoing = true;
            return RunOperation(totals);
        }

        public override bool IsArgumentOperation()
        {
            return true;
        }
    }
}
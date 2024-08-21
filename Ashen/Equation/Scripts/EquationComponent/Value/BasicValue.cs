using Ashen.DeliverySystem;
using System;

namespace Ashen.EquationSystem
{
    [Serializable]
    public class BasicValue : A_Value
    {
        public float value;

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            return value;
        }

        public override string Representation()
        {
            return value + "";
        }

        public override bool RequiresCaching()
        {
            return false;
        }

        public override bool Cache(I_DeliveryTool toolManager, Equation equation)
        {
            return true;
        }
    }
}
using Ashen.DeliverySystem;
using System;

namespace Ashen.EquationSystem
{
    [Serializable]
    public class UnknownValue : A_Value
    {
        public string value;

        public UnknownValue(string value)
        {
            this.value = value;
        }

        public override bool Cache(I_DeliveryTool toolManager, Equation equation)
        {
            return false;
        }

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            return 0;
        }

        public override string Representation()
        {
            return value;
        }

        public override bool RequiresCaching()
        {
            return false;
        }

        public override bool IsCachable()
        {
            return false;
        }

        public override bool InvalidComponent()
        {
            return true;
        }
    }
}
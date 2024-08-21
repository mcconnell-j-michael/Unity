using Ashen.DeliverySystem;

namespace Ashen.EquationSystem
{
    public abstract class A_Operation : A_EnumSO<A_Operation, Operations>, I_EquationComponent
    {
        public abstract float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments);
        public abstract string Representation();

        public virtual bool IsArgumentOperation()
        {
            return false;
        }

        public bool IsOperation()
        {
            return true;
        }

        public virtual bool RequiresCaching()
        {
            return false;
        }

        public virtual bool IsCachable()
        {
            return true;
        }

        public virtual bool Cache(I_DeliveryTool source, Equation equation)
        {
            return true;
        }

        public bool RequiresRebuild()
        {
            return false;
        }

        public bool InvalidComponent()
        {
            return false;
        }

        public I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks extraArguments)
        {
            return this;
        }
    }
}
using Ashen.DeliverySystem;
using System;

namespace Ashen.EquationSystem
{
    [Serializable]
    public abstract class A_Value : I_EquationComponent
    {
        public bool IsOperation()
        {
            return false;
        }

        public bool IsArgumentOperation()
        {
            return false;
        }

        public abstract string Representation();
        public abstract float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments);

        public abstract bool RequiresCaching();
        public abstract bool Cache(I_DeliveryTool toolManager, Equation equation);

        public virtual bool IsCachable()
        {
            return true;
        }

        public virtual bool RequiresRebuild()
        {
            return false;
        }

        public virtual I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks extraArguments)
        {
            return this;
        }

        public virtual bool InvalidComponent()
        {
            return false;
        }
    }
}
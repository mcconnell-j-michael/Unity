using Ashen.EquationSystem;

namespace Ashen.ToolSystem
{
    public abstract class A_ShiftableAttribute<Base, Current, Shift> : A_Shiftable<Base, Current, Shift>
    {
        public A_ShiftableAttribute(int size) : base(size) { }

        protected override Current Get(int x, DeliveryTool deliveryTool, DeliveryArgumentPacks deliveryArguments)
        {
            EquationArgumentPack arguments = deliveryArguments.GetPack<EquationArgumentPack>();
            AttributeLimiter limiter = arguments.GetPassthroughAttributeLimiter();
            OnChange(x, limiter);

            return Get(x, deliveryTool, deliveryArguments);
        }
    }
}
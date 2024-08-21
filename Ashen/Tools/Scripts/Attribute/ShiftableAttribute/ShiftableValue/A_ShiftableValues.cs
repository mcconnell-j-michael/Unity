using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public abstract class A_ShiftableValues<Base, Current, Shift> : A_Shiftable<Base, Current, Shift>
    {

        public A_ShiftableValues(int size) : base(size) { }

        public override void OnChange(int x, AttributeLimiter limiter)
        {
            valid[(int)limiter, x] = false;
        }

        protected override Current Get(int index, DeliveryTool deliveryTool, DeliveryArgumentPacks deliveryArguments)
        {
            Base baseValue = defaultAttributes[index];
            List<ShiftableChange<Shift>>[] changes = shifts[index];

            EquationArgumentPack arguments = deliveryArguments.GetPack<EquationArgumentPack>();

            AttributeLimiter limiter = arguments.GetPassthroughAttributeLimiter();

            Current total = CalculateBase(baseValue, deliveryTool, deliveryArguments);

            foreach (ShiftCategory category in limiter.GetShiftCategories())
            {
                Shift finalValue = InitializeValue();
                List<ShiftableChange<Shift>> shifts = changes[(int)category];
                foreach (ShiftableChange<Shift> shift in shifts)
                {
                    finalValue = AddShifts(finalValue, shift.shift, deliveryTool, deliveryArguments);
                }
                if (category.multiplier)
                {
                    total = Multiply(finalValue, total, deliveryTool, deliveryArguments);
                }
                else
                {
                    total = Add(finalValue, total, deliveryTool, deliveryArguments);
                }
                total = Limit(limiter, category, total);
            }
            return total;
        }

        protected virtual Shift InitializeValue()
        {
            return default;
        }
        protected abstract Current Limit(AttributeLimiter limiter, ShiftCategory shiftCategory, Current current);
        protected abstract Shift AddShifts(Shift first, Shift second, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments);
        protected abstract Current Multiply(Shift finalValue, Current total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments);
        protected abstract Current Add(Shift finalValue, Current total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments);
    }
}
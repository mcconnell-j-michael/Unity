using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class SingleShiftableAttribute<T> : A_ShiftableAttribute<T, T, T>
    {
        public SingleShiftableAttribute(int size) : base(size)
        {
        }

        protected override T CalculateBase(T baseValue, DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return baseValue;
        }

        public override void OnChange(int index, AttributeLimiter limiter)
        {
            currentAttributes[(int)limiter, index] = defaultAttributes[index];

            foreach (ShiftCategory category in limiter.GetShiftCategories())
            {
                List<ShiftableChange<T>> changes = shifts[index][(int)category];
                if (changes.Count == 0)
                {
                    continue;
                }
                ShiftableChange<T>? current = null;
                foreach (ShiftableChange<T> shiftableChange in changes)
                {
                    if (current == null || shiftableChange.priority < current.Value.priority)
                    {
                        current = shiftableChange;
                        continue;
                    }
                    if (shiftableChange.priority == current.Value.priority)
                    {
                        if (shiftableChange.source.CompareTo(current.Value.source) < 0)
                        {
                            current = shiftableChange;
                        }
                    }
                }

                currentAttributes[(int)limiter, index] = current.Value.shift;
            }

            valid[(int)limiter, index] = true;
        }
    }
}
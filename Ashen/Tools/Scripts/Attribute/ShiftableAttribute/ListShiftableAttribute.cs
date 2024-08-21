using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ListShiftableAttribute<T> : A_ShiftableAttribute<List<T>, List<T>, List<T>>
    {
        private bool enforceUniqueness;

        public ListShiftableAttribute(int size, bool enforceUniqueness = false) : base(size)
        {
            this.enforceUniqueness = enforceUniqueness;
            for (int x = 0; x < size; x++)
            {
                defaultAttributes[x] = new List<T>();
                foreach (AttributeLimiter limiter in AttributeLimiters.Instance)
                {
                    currentAttributes[(int)limiter, x] = new List<T>();
                }
            }
        }

        protected override List<T> CalculateBase(List<T> baseValue, DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return baseValue;
        }

        public override void OnChange(int index, AttributeLimiter limiter)
        {
            currentAttributes[(int)limiter, index] = defaultAttributes[index];
            ICollection<T> newAttributes = null;
            if (enforceUniqueness)
            {
                newAttributes = new HashSet<T>();
            }
            else
            {
                newAttributes = new List<T>();
            }

            foreach (T attribute in defaultAttributes[index])
            {
                newAttributes.Add(attribute);
            }

            foreach (ShiftCategory category in limiter.GetShiftCategories())
            {
                List<ShiftableChange<List<T>>> changes = shifts[index][(int)category];
                changes.Sort((a, b) =>
                {
                    if (a.priority == b.priority)
                    {
                        return string.Compare(a.source, b.source);
                    }
                    return b.priority - a.priority;
                });
                foreach (ShiftableChange<List<T>> shiftableChange in changes)
                {
                    if (shiftableChange.overwrite)
                    {
                        newAttributes.Clear();
                    }
                    foreach (T attribute in shiftableChange.shift)
                    {
                        newAttributes.Add(attribute);
                    }
                }
            }

            List<T> attributeList = new List<T>();
            attributeList.AddRange(newAttributes);
            currentAttributes[(int)limiter, index] = attributeList;
            valid[(int)limiter, index] = true;
        }
    }
}
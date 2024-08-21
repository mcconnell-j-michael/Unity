using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ashen.ToolSystem;
using Ashen.DeliverySystem;

namespace Ashen.DeliverySystem
{
    public abstract class A_Shiftable
    {
        public List<ShiftCategory> orderOfShifts;

        public Dictionary<ShiftCategory, ShiftPack> shifts;

        private float value = 0f;
        protected bool valid = false;

        public abstract float GetBase(I_DeliveryTool toolManager);

        public virtual void Initialize()
        {
            shifts = new Dictionary<ShiftCategory, ShiftPack>();
            for (int x = 0; x < orderOfShifts.Count; x++)
            {
                ShiftPack shiftPack = new ShiftPack();
                shifts.Add(orderOfShifts[x], shiftPack);
                shiftPack.Initialize();
            }
        }

        public void ApplyShift(ShiftCategory shiftCategory, string source, float value)
        {
            if (shifts.TryGetValue(shiftCategory, out ShiftPack shiftPack))
            {
                shiftPack.Apply(source, value);
                valid = false;
            }
        }

        public void RemoveShift(ShiftCategory shiftCategory, string source)
        {
            if (shifts.TryGetValue(shiftCategory, out ShiftPack shiftPack))
            {
                shiftPack.Clear(source);
                valid = false;
            }
        }

        public float GetValue(I_DeliveryTool toolManager)
        {
            if (!valid)
            {
                Calculate(toolManager);
            }
            return value;
        }

        public void Calculate(I_DeliveryTool toolManager)
        {
            float total = GetBase(toolManager);
            for (int x = 0; x < orderOfShifts.Count; x++)
            {
                ShiftPack shiftPack = shifts[orderOfShifts[x]];
                ShiftCategory category = ShiftCategories.Instance[x];
                if (category.multiplier)
                {
                    total *= (1f + shiftPack.GetValue());
                }
                else
                {
                    total += shiftPack.GetValue();
                }
            }
            value = total;
            valid = true;
        }

        public T Copy<T>(T shiftable) where T : A_Shiftable
        {
            shiftable.orderOfShifts = new List<ShiftCategory>(orderOfShifts);
            shiftable.shifts = new Dictionary<ShiftCategory, ShiftPack>();
            foreach (KeyValuePair<ShiftCategory, ShiftPack> kvp in shifts)
            {
                shiftable.shifts.Add(kvp.Key, kvp.Value.Copy());
            }
            return shiftable;
        }
    }
}
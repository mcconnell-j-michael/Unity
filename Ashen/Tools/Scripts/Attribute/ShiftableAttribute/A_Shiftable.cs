using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.EquationSystem;
using Ashen.ObjectPoolSystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public abstract class A_Shiftable<Base, Current, Shift>
    {
        protected Base[] defaultAttributes;
        protected Current[,] currentAttributes;
        protected List<ShiftableChange<Shift>>[][] shifts;
        protected bool[,] valid;

        public A_Shiftable(int size)
        {
            defaultAttributes = new Base[size];
            currentAttributes = new Current[AttributeLimiters.Count, size];
            valid = new bool[AttributeLimiters.Count, size];
            shifts = new List<ShiftableChange<Shift>>[size][];
            for (int y = 0; y < size; y++)
            {
                shifts[y] = new List<ShiftableChange<Shift>>[ShiftCategories.Count];
                for (int x = 0; x < ShiftCategories.Count; x++)
                {
                    shifts[y][x] = new List<ShiftableChange<Shift>>();
                }
            }
        }

        public void Initialize(string source, IEnumerable<I_EnumSO> enums, I_DeliveryTool deliveryTool, I_CombinedEnumListener invalidationListener)
        {
            InitializeInternal(source, enums, deliveryTool, invalidationListener);
        }

        protected virtual void InitializeInternal(string source, IEnumerable<I_EnumSO> enums, I_DeliveryTool deliveryTool, I_CombinedEnumListener invalidationListener) { }

        public void SetDefault(int index, Base attribute)
        {
            defaultAttributes[index] = attribute;
            OnChange(index);
        }

        public Current Get(int x, ToolManager toolManager, DeliveryArgumentPacks deliveryArgumentPacks)
        {
            DeliveryArgumentPacks deliveryArguments = deliveryArgumentPacks;
            EquationArgumentPack argument;
            if (deliveryArgumentPacks == null)
            {
                deliveryArguments = AGenericPool<DeliveryArgumentPacks>.Get();
                argument = deliveryArguments.GetPack<EquationArgumentPack>();
                argument.SetPassthroughAttributeLimiter(AttributeLimiters.Instance.DEFAULT_ATTRIBUTE_LIMITER);
            }
            else
            {
                argument = deliveryArguments.GetPack<EquationArgumentPack>();
            }

            if (argument.GetPassthroughAttributeLimiter() == null)
            {
                argument.SetPassthroughAttributeLimiter(AttributeLimiters.Instance.DEFAULT_ATTRIBUTE_LIMITER);
            }
            AttributeLimiter limiter = argument.GetPassthroughAttributeLimiter();
            if (!valid[(int)limiter, x])
            {
                Recalculate(x, toolManager.Get<DeliveryTool>(), limiter, deliveryArguments);
            }
            if (deliveryArgumentPacks == null)
            {
                AGenericPool<DeliveryArgumentPacks>.Release(deliveryArguments);
            }
            return currentAttributes[(int)limiter, x];
        }

        private void Recalculate(int x, DeliveryTool deliveryTool, AttributeLimiter limiter, DeliveryArgumentPacks arguments)
        {
            valid[(int)limiter, x] = true;
            currentAttributes[(int)limiter, x] = Get(x, deliveryTool, arguments);
        }

        public Current GetBase(int x, ToolManager toolManager)
        {
            return CalculateBase(defaultAttributes[x], toolManager.Get<DeliveryTool>(), null);
        }

        public void AddShift(I_DeliveryTool deliveryTool, I_CombinedEnumListener invalidationListener, I_EnumSO enumSO, int priority, ShiftCategory shiftCategory, string source, Shift shift, bool overwrite = false)
        {
            ShiftableChange<Shift> shiftChange = new()
            {
                priority = priority,
                source = source,
                shift = shift,
                overwrite = overwrite,
            };
            shifts[enumSO.GetIndex()][(int)shiftCategory].Add(shiftChange);
            AddShiftInternal(deliveryTool, shiftChange, invalidationListener, enumSO);
            OnChange(enumSO.GetIndex());
        }

        protected virtual void AddShiftInternal(I_DeliveryTool deliveryTool, ShiftableChange<Shift> shiftChange, I_CombinedEnumListener invalidationListener, I_EnumSO enumSO) { }

        public void RemoveShift(I_DeliveryTool deliveryTool, I_CombinedEnumListener invalidationListener, I_EnumSO enumSO, ShiftCategory shiftCategory, string source)
        {
            List<ShiftableChange<Shift>> foundShifts = shifts[enumSO.GetIndex()][(int)shiftCategory];
            for (int x = 0; x < foundShifts.Count; x++)
            {
                if (foundShifts[x].source == source)
                {
                    ShiftableChange<Shift> shiftChange = foundShifts[x];
                    foundShifts.RemoveAt(x);
                    x--;
                    RemoveShiftInternal(deliveryTool, shiftChange, invalidationListener, enumSO);
                }
            }
            OnChange(enumSO.GetIndex());
        }

        protected virtual void RemoveShiftInternal(I_DeliveryTool deliveryTool, ShiftableChange<Shift> shiftChange, I_CombinedEnumListener invalidationListener, I_EnumSO enumSO) { }

        public void OnChange(int x)
        {
            foreach (AttributeLimiter limiter in AttributeLimiters.Instance)
            {
                OnChange(x, limiter);
            }
        }

        public void Cleanup(I_DeliveryTool deliveryTool, I_InvalidationListener invalidationListener)
        {
            CleanupInternal(deliveryTool, invalidationListener);
        }

        protected virtual void CleanupInternal(I_DeliveryTool deliveryTool, I_InvalidationListener invalidationListener) { }

        public abstract void OnChange(int index, AttributeLimiter attributeLimiter);
        protected abstract Current CalculateBase(Base baseValue, DeliveryTool deliveryTool, DeliveryArgumentPacks arguments);
        protected abstract Current Get(int x, DeliveryTool deliveryTool, DeliveryArgumentPacks arguments);
    }
}
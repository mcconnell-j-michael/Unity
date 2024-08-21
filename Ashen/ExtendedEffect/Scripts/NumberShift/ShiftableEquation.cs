using Ashen.EquationSystem;
using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public class ShiftableEquation
    {
        public Equation BaseValue { get; set; }

        private ShiftPack[] shifts;
        public ShiftPack[] Shifts
        {
            get
            {
                if (!initialized) { Initialize(); }
                return shifts;
            }
        }

        private bool initialized = false;

        public virtual void Initialize()
        {
            shifts = new ShiftPack[ShiftCategories.Count];
            for (int x = 0; x < ShiftCategories.Count; x++)
            {
                ShiftPack shiftPack = new();
                shifts[x] = shiftPack;
                shiftPack.Initialize();
            }
            initialized = true;
        }

        public void ApplyShift(ShiftCategory shiftCategory, string source, float value)
        {
            if (!initialized) { Initialize(); }
            shifts[(int)shiftCategory].Apply(source, value);
        }

        public void RemoveShift(ShiftCategory shiftCategory, string source)
        {
            if (!initialized) { Initialize(); }
            shifts[(int)shiftCategory].Clear(source);
        }

        public float GetValue(I_DeliveryTool toolManager, DeliveryArgumentPacks extraArguments)
        {
            if (!initialized) { Initialize(); }
            return Calculate(toolManager, extraArguments);
        }

        public float Calculate(I_DeliveryTool toolManager, DeliveryArgumentPacks extraArguments)
        {
            if (!initialized) { Initialize(); }
            AttributeLimiter limiter = null;
            EquationArgumentPack equationArguments = extraArguments.GetPack<EquationArgumentPack>();
            if (extraArguments != null)
            {
                limiter = equationArguments.GetPassthroughAttributeLimiter();
            }
            if (limiter == null)
            {
                limiter = AttributeLimiters.Instance.DEFAULT_ATTRIBUTE_LIMITER;
            }
            if (!limiter.IsPassThrough() && extraArguments != null)
            {
                equationArguments.SetPassthroughAttributeLimiter(AttributeLimiters.Instance.DEFAULT_ATTRIBUTE_LIMITER);
            }

            float total = GetBase(toolManager, extraArguments);

            foreach (ShiftCategory category in limiter.GetShiftCategories())
            {
                ShiftPack shiftPack = shifts[(int)category];
                if (category.multiplier)
                {
                    total *= (1f + shiftPack.GetValue());
                }
                else
                {
                    total += shiftPack.GetValue();
                }
                total = limiter.Limit(category, total);
            }
            return total;
        }

        public ShiftableEquation Copy()
        {
            ShiftableEquation shiftable = new()
            {
                initialized = false
            };
            return shiftable;
        }

        public float GetBase(I_DeliveryTool toolManager, DeliveryArgumentPacks extraArguments)
        {
            if (!initialized) { Initialize(); }
            return BaseValue.Calculate(toolManager, extraArguments);
        }
    }
}
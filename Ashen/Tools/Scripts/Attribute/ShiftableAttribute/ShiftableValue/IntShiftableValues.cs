using Ashen.DeliverySystem;

namespace Ashen.ToolSystem
{
    public class IntShiftableValues : A_ShiftableValues<int, int, int>
    {
        public IntShiftableValues(int size) : base(size)
        {
        }

        protected override int Add(int finalValue, int total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return finalValue + total;
        }

        protected override int AddShifts(int first, int second, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return first + second;
        }

        protected override int Multiply(int finalValue, int total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return (finalValue + 1) * total;
        }
        protected override int Limit(AttributeLimiter limiter, ShiftCategory shiftCategory, int current)
        {
            return (int)limiter.Limit(shiftCategory, current);
        }

        protected override int CalculateBase(int baseValue, DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return baseValue;
        }
    }
}
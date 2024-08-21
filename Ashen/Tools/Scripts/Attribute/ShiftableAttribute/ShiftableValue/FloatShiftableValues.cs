using Ashen.DeliverySystem;

namespace Ashen.ToolSystem
{
    public class FloatShiftableValues : A_ShiftableValues<float, float, float>
    {
        public FloatShiftableValues(int size) : base(size)
        {
        }

        protected override float Add(float finalValue, float total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return finalValue + total;
        }

        protected override float AddShifts(float first, float second, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return first + second;
        }

        protected override float Multiply(float finalValue, float total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return (finalValue + 1.0f) * total;
        }

        protected override float Limit(AttributeLimiter limiter, ShiftCategory shiftCategory, float current)
        {
            return limiter.Limit(shiftCategory, current);
        }

        protected override float CalculateBase(float baseValue, DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return baseValue;
        }
    }
}
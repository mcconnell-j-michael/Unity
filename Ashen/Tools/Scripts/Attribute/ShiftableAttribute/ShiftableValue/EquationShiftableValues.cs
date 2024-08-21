using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.EquationSystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class EquationShiftableValues : A_ShiftableValues<Equation, float, I_DeliveryValue>
    {
        public EquationShiftableValues(int size) : base(size)
        {
        }

        protected override float Add(I_DeliveryValue finalValue, float total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            if (finalValue == null)
            {
                return total;
            }
            return finalValue.Build(deliveryTool, deliveryTool, arguments) + total;
        }

        protected override I_DeliveryValue InitializeValue()
        {
            return new SimpleValue(0);
        }

        protected override I_DeliveryValue AddShifts(I_DeliveryValue first, I_DeliveryValue second, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return new SimpleValue(first.Build(deliveryTool, deliveryTool, arguments) + second.Build(deliveryTool, deliveryTool, arguments));
        }

        protected override float Multiply(I_DeliveryValue finalValue, float total, I_DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return (finalValue.Build(deliveryTool, deliveryTool, arguments) + 1.0f) * total;
        }

        protected override float Limit(AttributeLimiter limiter, ShiftCategory shiftCategory, float current)
        {
            return limiter.Limit(shiftCategory, current);
        }

        protected override float CalculateBase(Equation baseValue, DeliveryTool deliveryTool, DeliveryArgumentPacks arguments)
        {
            return baseValue.Calculate(deliveryTool, arguments);
        }

        protected override void AddShiftInternal(I_DeliveryTool deliveryTool, ShiftableChange<I_DeliveryValue> shiftChange, I_CombinedEnumListener invalidationListener, I_EnumSO enumSO)
        {
            shiftChange.shift.OnRegister(deliveryTool, invalidationListener, enumSO);
        }

        protected override void RemoveShiftInternal(I_DeliveryTool deliveryTool, ShiftableChange<I_DeliveryValue> shiftChange, I_CombinedEnumListener invalidationListener, I_EnumSO enumSO)
        {
            shiftChange.shift.OnDeregister(deliveryTool, invalidationListener, enumSO);
        }

        protected override void InitializeInternal(string source, IEnumerable<I_EnumSO> enums, I_DeliveryTool deliveryTool, I_CombinedEnumListener invalidationListener)
        {
            for (int i = 0; i < defaultAttributes.Length; i++)
            {
                Equation baseEquation = defaultAttributes[i];
                I_EnumSO foundEnum = null;
                foreach (I_EnumSO enumSO in enums)
                {
                    if (enumSO.GetIndex() == i)
                    {
                        foundEnum = enumSO;
                        break;
                    }
                }
                if (foundEnum != null)
                {
                    baseEquation.AddInvalidationListener(
                        deliveryTool,
                        invalidationListener,
                        new InvalidationIdentifier() { source = source, key = foundEnum.ToString(), enumKey = foundEnum }
                    );
                }
            }
        }

        protected override void CleanupInternal(I_DeliveryTool deliveryTool, I_InvalidationListener invalidationListener)
        {
            foreach (Equation baseEquation in defaultAttributes)
            {
                baseEquation.RemoveInvalidationListener(deliveryTool, invalidationListener);
            }
        }
    }
}
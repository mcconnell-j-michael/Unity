using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public class TearDownThresholdValue : A_ThresholdValue
    {
        public TearDownThresholdValue(DerivedAttribute maxValue, ResourceValue resourceValue, I_ThresholdDecayManager manager, OnThresholdMet onThresholdMet, bool retainRatioOnHigher, bool retainRatioOnLower)
            : base(maxValue, resourceValue, manager, onThresholdMet, retainRatioOnHigher, retainRatioOnLower) { }

        public override void ClearDamage()
        {
            RemoveAmount(currentValue);
        }

        public override void Init(ToolManager toolManager)
        {
            base.Init(toolManager);
            currentValue = (int)maxValue.Get(toolManager.Get<DeliveryTool>());
        }

        public override bool IsThresholdMet()
        {
            return currentValue == 0;
        }

        public override void ApplyAmount(int damage)
        {
            if (!enabled)
            {
                return;
            }
            int previous = currentValue;
            currentValue -= damage;
            int overflow = 0;
            if (currentValue < 0)
            {
                overflow = -currentValue;
                currentValue = 0;
            }
            HandleChange(previous, overflow, true);
        }

        public override void RemoveAmount(int damage)
        {
            if (!enabled)
            {
                return;
            }
            int previous = currentValue;
            currentValue += damage;
            if (currentValue > currentMaxValue)
            {
                currentValue = currentMaxValue;
            }
            HandleChange(previous, 0, false);
        }

        public override int GetBaseValue()
        {
            return currentMaxValue;
        }

        public override int GetLimit()
        {
            return 0;
        }

        public override void SetAmount(int total)
        {
            if (!enabled || total == currentValue)
            {
                return;
            }
            if (total > currentValue)
            {
                RemoveAmount(total - currentValue);
            }
            if (total < currentValue)
            {
                ApplyAmount(currentValue - total);
            }
        }

        public override void ApplyTempAmount(ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            tempValue.TempValue = -tempValue.TempValue;
            tempChanges[(int)category].Add(tempValue);
            HandleChange(currentValue, 0, true);
        }

        public override void RemoveTempAmount(ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            tempChanges[(int)category].Add(tempValue);
            HandleChange(currentValue, 0, false);
        }
    }
}
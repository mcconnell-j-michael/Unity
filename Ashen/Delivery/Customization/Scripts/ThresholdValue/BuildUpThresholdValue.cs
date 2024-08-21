using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public class BuildUpThresholdValue : A_ThresholdValue
    {
        public BuildUpThresholdValue(DerivedAttribute maxValue, ResourceValue resourceValue, I_ThresholdDecayManager manager, OnThresholdMet onThresholdMet, bool retainRatioOnHigher, bool retainRatioOnLower)
            : base(maxValue, resourceValue, manager, onThresholdMet, retainRatioOnHigher, retainRatioOnLower) { }

        public override void ClearDamage()
        {
            RemoveAmount(currentMaxValue);
        }

        public override void Init(ToolManager toolManager)
        {
            base.Init(toolManager);
            currentValue = 0;
        }

        public override bool IsThresholdMet()
        {
            return currentValue == currentMaxValue;
        }

        public override void ApplyAmount(int damage)
        {
            if (!enabled)
            {
                return;
            }
            int previous = currentValue;
            currentValue += damage;
            int overflow = 0;
            if (currentValue > currentMaxValue)
            {
                overflow = currentValue - currentMaxValue;
                currentValue = currentMaxValue;
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
            currentValue -= damage;
            if (currentValue < 0)
            {
                currentValue = 0;
            }
            HandleChange(previous, 0, false);
        }

        public override int GetBaseValue()
        {
            return 0;
        }

        public override int GetLimit()
        {
            return currentMaxValue;
        }

        public override void SetAmount(int total)
        {
            if (!enabled || total == currentValue)
            {
                return;
            }
            if (total > currentValue)
            {
                ApplyAmount(total - currentValue);
            }
            if (total < currentValue)
            {
                RemoveAmount(currentValue - total);
            }
        }

        public override void ApplyTempAmount(ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            tempChanges[(int)category].Add(tempValue);
            HandleChange(currentValue, 0, true);
        }

        public override void RemoveTempAmount(ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            tempValue.TempValue = -tempValue.TempValue;
            tempChanges[(int)category].Add(tempValue);
            HandleChange(currentValue, 0, false);
        }
    }
}
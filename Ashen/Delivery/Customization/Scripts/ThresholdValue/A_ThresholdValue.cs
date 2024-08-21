using Ashen.EnumSystem;
using Ashen.EquationSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public abstract class A_ThresholdValue : I_EnumCacheable, I_DamageListener, I_TriggerListener
    {
        public abstract void ClearDamage();
        public abstract void ApplyAmount(int damage);
        public abstract void RemoveAmount(int damage);
        public abstract void ApplyTempAmount(ThresholdValueTempCategory category, TempValueContainer tempValue);
        public abstract void RemoveTempAmount(ThresholdValueTempCategory category, TempValueContainer tempValue);
        public abstract void SetAmount(int total);
        public abstract bool IsThresholdMet();
        public abstract int GetBaseValue();
        public abstract int GetLimit();

        public virtual void Init(ToolManager toolManager)
        {
            this.toolManager = toolManager;
            currentMaxValue = (int)maxValue.Get(toolManager.Get<DeliveryTool>());
            AttributeTool attributeTool = toolManager.Get<AttributeTool>();
            attributeTool.Cache(maxValue, this);
            if (decayManager != null)
            {
                decayManager.Init(this, toolManager.Get<DeliveryTool>());
            }
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            if (triggerBuilders != null)
            {
                for (int x = 0; x < triggerBuilders.Length; x++)
                {
                    if (triggerBuilders[x] != null)
                    {
                        triggerTool.RegisterTriggerListener(ExtendedEffectTriggers.Instance[x], this);
                    }
                }
            }
        }

        private List<I_ThresholdListener> thresholdMetListener;
        private List<I_ThresholdListener> thresholdChangeListener;

        protected DerivedAttribute maxValue;
        [HideInEditorMode, ShowInInspector, ReadOnly]
        protected int currentMaxValue;

        protected bool retainRatioOnMaxHigher;
        protected bool retainRatioOnMaxLower;
        protected OnThresholdMet onThresholdMet;

        protected ResourceValue resourceValue;

        protected List<TempValueContainer>[] tempChanges;

        public int currentValue;
        protected ToolManager toolManager;
        public bool enabled;
        public I_ThresholdDecayManager decayManager;

        public ThresholdTriggerEventBuilder[] triggerBuilders;

        public A_ThresholdValue(DerivedAttribute maxValue, ResourceValue resourceValue, I_ThresholdDecayManager manager, OnThresholdMet onThresholdMet, bool retainRatioOnHigher, bool retainRatioOnLower)
        {
            this.maxValue = maxValue;
            this.retainRatioOnMaxHigher = retainRatioOnHigher;
            this.retainRatioOnMaxLower = retainRatioOnLower;
            enabled = true;
            this.decayManager = manager;
            thresholdChangeListener = new List<I_ThresholdListener>();
            thresholdMetListener = new List<I_ThresholdListener>();
            this.onThresholdMet = onThresholdMet;
            this.resourceValue = resourceValue;
            tempChanges = new List<TempValueContainer>[ThresholdValueTempCategories.Count];
            foreach (ThresholdValueTempCategory category in ThresholdValueTempCategories.Instance)
            {
                tempChanges[(int)category] = new List<TempValueContainer>();
            }
        }

        public ThresholdEventValue GetCurrentState()
        {
            return GenerateThresholdEvent(currentValue, false);
        }

        protected ThresholdEventValue GenerateThresholdEvent(int previous, bool damageTaken)
        {
            int[] tempChanges = new int[ThresholdValueTempCategories.Count];
            foreach (ThresholdValueTempCategory category in ThresholdValueTempCategories.Instance)
            {
                int tempValue = 0;
                List<TempValueContainer> containers = this.tempChanges[(int)category];
                foreach (TempValueContainer tempContainer in containers)
                {
                    tempValue += tempContainer.TempValue;
                }
                tempChanges[(int)category] = tempValue;
            }
            return new ThresholdEventValue
            {
                resourceValue = resourceValue,
                currentValue = currentValue,
                max = currentValue >= currentMaxValue,
                maxValue = currentMaxValue,
                min = currentValue <= 0,
                previousValue = previous,
                damageTaken = damageTaken,
                tempValues = tempChanges
            };
        }

        public void HandleChange(int previous, int overflow, bool apply)
        {
            ThresholdEventValue value = GenerateThresholdEvent(previous, true);
            ReportChange(value);
            if (currentValue == GetLimit() && previous != currentValue)
            {
                ReportThresholdMet(value);
                switch (this.onThresholdMet)
                {
                    case OnThresholdMet.CARRY_OVER:
                        currentValue = GetBaseValue();
                        if (apply)
                        {
                            ApplyAmount(overflow);
                        }
                        else
                        {
                            RemoveAmount(overflow);
                        }
                        ThresholdEventValue carryOverValue = GenerateThresholdEvent(GetLimit(), apply);
                        ReportChange(carryOverValue);
                        break;
                    case OnThresholdMet.MAINTAIN:
                        // Do Nothing
                        break;
                    case OnThresholdMet.RESET:
                        currentValue = GetBaseValue();
                        ThresholdEventValue resetValue = GenerateThresholdEvent(GetLimit(), apply);
                        ReportChange(resetValue);
                        break;
                }
            }
        }

        public int CalculateValueWithLimit(int tempValue)
        {
            int newValue = currentValue + tempValue;
            if (newValue < 0)
            {
                return 0;
            }
            if (newValue > currentMaxValue)
            {
                return currentMaxValue;
            }
            return newValue;
        }

        public void ClearTempValues()
        {
            foreach (ThresholdValueTempCategory category in ThresholdValueTempCategories.Instance)
            {
                tempChanges[(int)category].Clear();
            }
            HandleChange(currentValue, 0, false);
        }

        public void ClearTempValues(ThresholdValueTempCategory category)
        {
            tempChanges[(int)category].Clear();
            HandleChange(currentValue, 0, false);
        }

        public void DeleteTempValue(ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            tempChanges[(int)category].Remove(tempValue);
            HandleChange(currentValue, 0, false);
        }

        public void ReportChange(ThresholdEventValue value)
        {
            value.eventType = ThresholdEventType.ON_CHANGE;
            foreach (I_ThresholdListener listener in thresholdChangeListener)
            {
                listener.OnThresholdEvent(value);
            }
        }

        public float Percentage()
        {
            return currentValue / (float)currentMaxValue;
        }

        public void ReportThresholdMet(ThresholdEventValue value)
        {
            value.eventType = ThresholdEventType.ON_THRESHOLD_MET;
            foreach (I_ThresholdListener listener in thresholdMetListener)
            {
                listener.OnThresholdEvent(value);
            }
            if (resourceValue.threshold.triggerOnThresholdMet == null)
            {
                return;
            }
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            if (triggerTool == null)
            {
                return;
            }
            triggerTool.Trigger(resourceValue.threshold.triggerOnThresholdMet);
        }

        public void RegisterThresholdMetListener(I_ThresholdListener listener)
        {
            thresholdMetListener.Add(listener);
        }

        public void UnRegesterThresholdMetListener(I_ThresholdListener listener)
        {
            thresholdMetListener.Remove(listener);
        }

        public void RegiserThresholdChangeListener(I_ThresholdListener listener)
        {
            thresholdChangeListener.Add(listener);
        }

        public void UnRegesterThresholdChangeListener(I_ThresholdListener listener)
        {
            thresholdChangeListener.Remove(listener);
        }

        public void Disable()
        {
            ClearDamage();
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Reset(int currentValue, bool enabled)
        {
            this.currentValue = currentValue;
            this.enabled = enabled;
            this.currentMaxValue = (int)maxValue.Get(toolManager.Get<DeliveryTool>());
            if (this.currentValue <= 0)
            {
                this.currentValue = 0;
            }
            if (currentValue >= currentMaxValue)
            {
                this.currentValue = currentMaxValue;
            }
            ThresholdEventValue value = GetCurrentState();
            ReportChange(value);
        }

        public void Recalculate(I_DeliveryTool deliveryTool, EquationArgumentPack extraArguments)
        {
            Recalculate(maxValue, deliveryTool, extraArguments);
        }
        public void Recalculate(I_EnumSO enumValue, I_DeliveryTool deliveryTool)
        {
            if (enumValue is DerivedAttribute attribute)
            {
                Recalculate(attribute, deliveryTool, null);
            }
        }

        public void Recalculate(DerivedAttribute enumValue, I_DeliveryTool deliveryTool, EquationArgumentPack extraArguments)
        {
            if (!toolManager)
            {
                return;
            }
            float oldPercentage = this.currentValue / (float)currentMaxValue;
            int newMaxValue = (int)maxValue.Get(toolManager.Get<DeliveryTool>());
            int currentValue = this.currentValue;
            if (newMaxValue > currentMaxValue)
            {
                currentValue = retainRatioOnMaxHigher ? ((int)(newMaxValue * oldPercentage)) : this.currentValue;
            }
            if (newMaxValue < currentMaxValue)
            {
                currentValue = retainRatioOnMaxLower ? ((int)(newMaxValue * oldPercentage)) : this.currentValue;
            }
            currentMaxValue = newMaxValue;
            if (currentValue <= 0)
            {
                currentValue = 0;
            }
            if (currentValue >= newMaxValue)
            {
                currentValue = newMaxValue;
            }
            int previous = this.currentValue;
            this.currentValue = currentValue;
            ThresholdEventValue value = GenerateThresholdEvent(previous, false);
            if (IsThresholdMet())
            {
                ReportThresholdMet(value);
            }
            ReportChange(value);
        }

        public void OnDamageEvent(DamageEvent damageEvent)
        {
            int totalDamage = damageEvent.Total;
            if (totalDamage < 0)
            {
                RemoveAmount(-totalDamage);
            }
            else
            {
                ApplyAmount(totalDamage);
            }
        }

        public void OnTrigger(ExtendedEffectTrigger trigger)
        {
            ThresholdTriggerEventBuilder builder = triggerBuilders[(int)trigger];
            if (builder == null)
            {
                return;
            }
            if (builder.type == ThresholdTriggerEventType.MAX)
            {
                int maxValue = (int)this.maxValue.Get(toolManager.Get<DeliveryTool>());
                this.Reset(maxValue, this.enabled);
            }
            else if (builder.type == ThresholdTriggerEventType.MIN)
            {
                this.Reset(0, this.enabled);
            }
            else if (builder.type == ThresholdTriggerEventType.VALUE)
            {
                this.Reset((int)builder.value.Calculate(toolManager.Get<DeliveryTool>()), this.enabled);
            }
        }
    }
}
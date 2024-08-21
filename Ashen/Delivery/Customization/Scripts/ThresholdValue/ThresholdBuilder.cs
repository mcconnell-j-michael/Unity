using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class ThresholdBuilder
    {
        [EnumToggleButtons, HideLabel]
        public ThresholdType thresholdType;
        [OdinSerialize]
        public DerivedAttribute maxValue;
        [ToggleGroup("hasDecay")]
        public bool hasDecay;
        [ToggleGroup("hasDecay"), HideLabel, Title("Delay")]
        public DerivedAttribute decayDelay;
        [ToggleGroup("hasDecay"), HideLabel, Title("Rate")]
        public DerivedAttribute decayRate;
        [FoldoutGroup("Advanced"), Title("Max Increase"), HideLabel, EnumToggleButtons]
        public BoundChangeOptions maxIncrease;
        [FoldoutGroup("Advanced"), Title("Max Decrease"), HideLabel, EnumToggleButtons]
        public BoundChangeOptions maxDecrease;

        [EnumToggleButtons, HideLabel]
        public OnThresholdMet onThresholdMet;
        [OdinSerialize]
        public ExtendedEffectTrigger triggerOnThresholdMet;

        [OdinSerialize]
        public Dictionary<ExtendedEffectTrigger, ThresholdTriggerEventBuilder> triggerEvents;

        public A_ThresholdValue BuildThresholdValue(ToolManager toolManager, ResourceValue resourceValue)
        {
            A_ThresholdValue value = null;
            ThresholdDecayManager manager = null;
            if (hasDecay && !TimeRegistry.Instance.turnBased)
            {
                manager = new ThresholdDecayManager(decayDelay, decayRate);
            }
            switch (thresholdType)
            {
                case ThresholdType.BUILD_UP:
                    value = new BuildUpThresholdValue(
                        maxValue,
                        resourceValue,
                        manager,
                        onThresholdMet,
                        maxIncrease == BoundChangeOptions.RetainRatio,
                        maxDecrease == BoundChangeOptions.RetainValue);
                    break;
                case ThresholdType.TEAR_DOWN:
                    value = new TearDownThresholdValue(
                        maxValue,
                        resourceValue,
                        manager,
                        onThresholdMet,
                        maxIncrease == BoundChangeOptions.RetainRatio,
                        maxDecrease == BoundChangeOptions.RetainValue);
                    break;
            }
            if (value == null)
            {
                Logger.ErrorLog("No threshold value type was configured for threshold builder");
                return null;
            }
            value.triggerBuilders = new ThresholdTriggerEventBuilder[ExtendedEffectTriggers.Count];
            if (triggerEvents != null)
            {
                foreach (KeyValuePair<ExtendedEffectTrigger, ThresholdTriggerEventBuilder> pair in triggerEvents)
                {
                    value.triggerBuilders[(int)pair.Key] = pair.Value;
                }
            }
            return value;
        }
    }

    public enum ThresholdType
    {
        BUILD_UP, TEAR_DOWN
    }

    public enum BoundChangeOptions
    {
        RetainRatio, RetainValue
    }

    public enum OnThresholdMet
    {
        RESET, CARRY_OVER, MAINTAIN
    }
}
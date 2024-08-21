using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public struct ThresholdEventValue
    {
        public ResourceValue resourceValue;
        public ThresholdEventType eventType;
        public int previousValue;
        public int currentValue;
        public int maxValue;
        public int[] tempValues;
        public bool max;
        public bool min;
        public bool damageTaken;
    }
}
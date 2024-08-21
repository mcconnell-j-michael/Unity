using System;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    /**
     * A TempFilter is one that can be disabled after a condition is met. This could be for a number of reasons.
     * Ex. A damage shield that only blocks x amount of damage before it is removed or a filter that will be removed
     * after it has successfully been activated x amount of times
     **/
    [Serializable]
    public abstract class A_TempFilter : I_Filter
    {
        private bool disabled;

        public A_TempFilter() { }

        public abstract bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult);

        public void Disable()
        {
            disabled = true;
        }

        public virtual bool Enabled()
        {
            return !disabled;
        }

        public A_TempFilter(SerializationInfo info, StreamingContext context)
        {
            disabled = info.GetBoolean(nameof(disabled));
        }

        protected virtual void GetObjectDataInternal(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(disabled), disabled);
        }
    }
}
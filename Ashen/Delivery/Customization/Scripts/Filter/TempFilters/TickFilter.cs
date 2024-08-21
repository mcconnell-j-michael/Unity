using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * A TickFilter will apply x amount of times before it will automatically get removed
     **/
    [Serializable]
    public class TickFilter : A_TempFilter
    {
        private I_Filter filter = default;
        private int numTicks;

        public TickFilter() { }

        public TickFilter(I_Filter filter, int numTicks)
        {
            this.filter = filter;
            this.numTicks = numTicks;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            if (filter.Apply(owner, target, deliveryArgumentsPack, deliveryResult))
            {
                numTicks--;
                return true;
            }
            return false;
        }

        public override bool Enabled()
        {
            return base.Enabled() && numTicks > 0 && filter.Enabled();
        }

        public TickFilter(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            filter = StaticUtilities.LoadInterfaceValue<I_Filter>(info, nameof(filter));
            numTicks = info.GetInt32(nameof(numTicks));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectDataInternal(info, context);
            StaticUtilities.SaveInterfaceValue(info, nameof(filter), filter);
            info.AddValue(nameof(numTicks), numTicks);
        }
    }
}

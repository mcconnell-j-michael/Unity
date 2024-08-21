using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class FilterWithCondition : A_BaseFilter
    {
        private I_ConditionalFilter condition;
        private I_Filter filter;

        public FilterWithCondition(I_ConditionalFilter condition, I_Filter filter)
        {
            this.condition = condition;
            this.filter = filter;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            if (condition.Check(owner, target, deliveryArgumentsPack, deliveryResult))
            {
                return filter.Apply(owner, target, deliveryArgumentsPack, deliveryResult);
            }
            return false;
        }

        public FilterWithCondition(SerializationInfo info, StreamingContext context)
        {
            condition = StaticUtilities.LoadInterfaceValue<I_ConditionalFilter>(info, nameof(condition));
            filter = StaticUtilities.LoadInterfaceValue<I_Filter>(info, nameof(filter));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(condition), condition);
            StaticUtilities.SaveInterfaceValue(info, nameof(filter), filter);
        }
    }
}
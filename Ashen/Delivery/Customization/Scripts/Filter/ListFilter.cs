using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * The ListFilter has the ability to apply a number of baseFilters. A list filter is marked as enabled as long
     * as all of the filters in its list are enabled. If one filter is not enabled, then the list filter is disabled
     **/
    [Serializable]
    public class ListFilter : I_Filter
    {
        [OdinSerialize]
        private List<A_BaseFilter> filters;

        public ListFilter() { }

        public ListFilter(List<A_BaseFilter> filters)
        {
            this.filters = filters;
        }

        public bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            bool applied = false;
            foreach (I_Filter filter in filters)
            {
                applied = applied || filter.Apply(owner, target, deliveryArgumentsPack, deliveryResult);
            }
            return applied;
        }

        public bool Enabled()
        {
            bool enabled = true;
            foreach (I_Filter filter in filters)
            {
                enabled = enabled && filter.Enabled();
            }
            return enabled;
        }

        public ListFilter(SerializationInfo info, StreamingContext context)
        {
            filters = StaticUtilities.LoadList(info, nameof(filters), (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<A_BaseFilter>(info, name);
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(filters), filters, (string name, A_BaseFilter effect) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, effect);
            });
        }
    }
}
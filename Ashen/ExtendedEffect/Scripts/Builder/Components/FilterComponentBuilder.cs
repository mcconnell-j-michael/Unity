using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class FilterComponentBuilder : I_ComponentBuilder
    {
        [Serializable]
        public enum FilterType
        {
            PRE_DEFENSIVE,
            POST_DEFENSIVE,
            PRE_OFFENSIVE,
            POST_OFFENSIVE
        }

        [OdinSerialize, EnumToggleButtons, HideLabel, Title("Type")]
        private FilterType filterType = default;

        [OdinSerialize, HideLabel, Title("Filter"), InlineProperty, Indent]
        private I_FilterBuilder filter = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            I_Filter filter = this.filter.Build(owner, target, deliveryArguments);
            if (filter == null)
            {
                return null;
            }
            return new FilterComponent(filter, filterType);
        }

        public FilterComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            filterType = (FilterType)info.GetValue(nameof(filterType), typeof(FilterType));
            filter = StaticUtilities.LoadInterfaceValue<I_FilterBuilder>(info, nameof(filter));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(filterType), filterType);
            StaticUtilities.SaveInterfaceValue(info, nameof(filter), filter);
        }
    }
}
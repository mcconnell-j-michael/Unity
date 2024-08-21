using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static Ashen.DeliverySystem.FilterComponentBuilder;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class FilterComponent : A_SimpleComponent
    {
        private FilterType filterType;
        private I_Filter filter;

        public FilterComponent() { }

        public FilterComponent(I_Filter filter, FilterType filterType)
        {
            this.filter = filter;
            this.filterType = filterType;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            KeyContainer<I_Filter> filterContainer = new KeyContainer<I_Filter>(filter, container.key);
            if (deliveryTool != null)
            {
                switch (filterType)
                {
                    case FilterType.PRE_DEFENSIVE:
                        deliveryTool.GetPreDefensiveFilters().Add(filterContainer);
                        break;
                    case FilterType.PRE_OFFENSIVE:
                        deliveryTool.GetPreOffensiveFilters().Add(filterContainer);
                        break;
                    case FilterType.POST_DEFENSIVE:
                        deliveryTool.GetPostDefensiveFilters().Add(filterContainer);
                        break;
                    case FilterType.POST_OFFENSIVE:
                        deliveryTool.GetPostOffensiveFilters().Add(filterContainer);
                        break;
                }
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            if (deliveryTool != null)
            {
                deliveryTool.RemoveFilter(filterType, container.key);
            }
        }

        public FilterComponent(SerializationInfo info, StreamingContext context)
        {
            filterType = (FilterType)info.GetValue(nameof(filterType), typeof(FilterType));
            filter = StaticUtilities.LoadInterfaceValue<I_Filter>(info, nameof(filter));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(filter), filter);
            info.AddValue(nameof(filterType), filterType);
        }
    }
}

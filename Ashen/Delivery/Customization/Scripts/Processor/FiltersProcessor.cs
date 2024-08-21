using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.DeliverySystem
{
    public class FiltersProcessor : I_DeliveryProcessor
    {
        public enum FILTER_CATEGORY
        {
            PRE_FILTER, POST_FILTER, PRE_DEFENSIVE_FILTER, POST_DEFENSIVE_FILTER, PRE_OFFENSIVE_FILTER, POST_OFFENSIVE_FILTER
        }

        [ValueDropdown("@Enum.GetValues(typeof(" + nameof(FILTER_CATEGORY) + "))")]
        public FILTER_CATEGORY filterCategory;

        public void Process(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            switch (filterCategory)
            {
                case FILTER_CATEGORY.PRE_FILTER:
                    {
                        FilterArgumentPack filterArgumentPack = deliveryArguments.GetPack<FilterArgumentPack>();
                        ApplyFilters(filterArgumentPack.GetPreFilters(), owner, target, deliveryArguments);
                        break;
                    }
                case FILTER_CATEGORY.PRE_DEFENSIVE_FILTER:
                    {
                        if (target != null)
                        {
                            DeliveryTool deliveryTool = target as DeliveryTool;
                            ApplyFilters(deliveryTool.GetPreDefensiveFilters(), target, owner, deliveryArguments);
                        }
                        break;
                    }
                case FILTER_CATEGORY.PRE_OFFENSIVE_FILTER:
                    {
                        if (owner != null)
                        {
                            DeliveryTool deliveryTool = owner as DeliveryTool;
                            ApplyFilters(deliveryTool.GetPreOffensiveFilters(), owner, target, deliveryArguments);
                        }
                        break;
                    }
                case FILTER_CATEGORY.POST_FILTER:
                    {
                        FilterArgumentPack filterArgumentPack = deliveryArguments.GetPack<FilterArgumentPack>();
                        ApplyFilters(filterArgumentPack.GetPostFilters(), owner, target, deliveryArguments);
                        break;
                    }
                case FILTER_CATEGORY.POST_DEFENSIVE_FILTER:
                    {
                        if (target != null)
                        {
                            DeliveryTool deliveryTool = target as DeliveryTool;
                            ApplyFilters(deliveryTool.GetPostDefensiveFilters(), target, owner, deliveryArguments);
                        }
                        break;
                    }
                case FILTER_CATEGORY.POST_OFFENSIVE_FILTER:
                    {
                        if (owner != null)
                        {
                            DeliveryTool deliveryTool = owner as DeliveryTool;
                            ApplyFilters(deliveryTool.GetPostOffensiveFilters(), owner, target, deliveryArguments);
                        }
                        break;
                    }
            }
        }

        private void ApplyFilters(List<KeyContainer<I_Filter>> filters, I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack)
        {
            if (filters == null || filters.Count == 0)
            {
                return;
            }
            DeliveryResultsArgumentPack deliveryResultsArgumentPack = deliveryArgumentsPack.GetPack<DeliveryResultsArgumentPack>();
            DeliveryResultPack drp = deliveryResultsArgumentPack.GetDeliveryResultPack();
            for (int x = 0; x < filters.Count; x++)
            {
                I_Filter filter = filters[x].source;
                if (filter.Enabled())
                {
                    filter.Apply(owner, target, deliveryArgumentsPack, drp);
                }
                else
                {
                    UnorderedListUtility<KeyContainer<I_Filter>>.RemoveAt(filters, x);
                    x--;
                }

            }
        }
    }
}
using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class ListFilterBuilder : MonoBehaviour
    {
        [OdinSerialize]
        private List<I_FilterBuilder> filters = default;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            List<A_BaseFilter> newFilters = new List<A_BaseFilter>(filters.Count);
            for (int x = 0; x < filters.Count; x++)
            {
                I_FilterBuilder filter = filters[x];
                newFilters.Add(filter.Build(owner, target, arguments) as A_BaseFilter);
            }
            return new ListFilter(newFilters);
        }
    }
}
using System;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A conditional filter is a wrapper for a BaseFilter, that will only apply the BaseFilter if its 
     * Check method returns true. ConditionalFilters must override this class
     **/
    [Serializable]
    public abstract class A_ConditionalFilterBuilder : I_FilterBuilder
    {
        [SerializeField]
        private I_FilterBuilder filter;

        public A_ConditionalFilterBuilder() { }

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            I_ConditionalFilter condition = BuildCondition(owner, target, arguments);
            I_Filter builtFilter = filter.Build(owner, target, arguments);
            return new FilterWithCondition(condition, builtFilter);
        }

        public abstract I_ConditionalFilter BuildCondition(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments);
    }
}
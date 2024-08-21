using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class AttributeLimiter : A_EnumSO<AttributeLimiter, AttributeLimiters>
    {
        [OdinSerialize, HideIf(nameof(IsDefault))]
        private bool overrideShiftCategoryOrder;
        [OdinSerialize, HideIf("@ !" + nameof(overrideShiftCategoryOrder) + " && !" + nameof(IsDefault) + "()")]
        private List<ShiftCategory> shiftCategoryOrder;

        [OdinSerialize]
        private Dictionary<ShiftCategory, ShiftLimit> limiterDefinitions;
        [OdinSerialize]
        private bool passThroughLimiter;

        public bool IsDefault()
        {
            return this == Default();
        }

        public List<ShiftCategory> GetShiftCategories()
        {
            if (overrideShiftCategoryOrder || IsDefault())
            {
                if (shiftCategoryOrder.IsNullOrEmpty() && !IsDefault())
                {
                    return Default().shiftCategoryOrder;
                }
                return shiftCategoryOrder;
            }
            else
            {
                return Default().shiftCategoryOrder;
            }
        }

        public ShiftLimit GetShiftLimit(ShiftCategory category)
        {
            if (limiterDefinitions == null)
            {
                if (IsDefault())
                {
                    return null;
                }
                else
                {
                    return Default().GetShiftLimit(category);
                }
            }
            if (limiterDefinitions.TryGetValue(category, out ShiftLimit limit))
            {
                return limit;
            }
            return Default().GetShiftLimit(category);
        }

        public float Limit(ShiftCategory category, float original)
        {
            ShiftLimit limit = GetShiftLimit(category);
            if (limit == null)
            {
                return original;
            }
            return limit.Limit(original);
        }

        public bool IsPassThrough()
        {
            return passThroughLimiter;
        }

        public AttributeLimiter Default()
        {
            return AttributeLimiters.Instance.DEFAULT_ATTRIBUTE_LIMITER;
        }
    }
}
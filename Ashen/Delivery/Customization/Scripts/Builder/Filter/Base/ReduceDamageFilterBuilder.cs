using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class ReduceDamageFilterBuilder : I_FilterBuilder
    {
        [OdinSerialize, Hide, Title("Damage Types")]
        private List<DamageType> damageTypes = default;
        [OdinSerialize, HideIf(nameof(percentage))]
        private I_DeliveryValue amountToReduceBy = default;
        [OdinSerialize, HideIf(nameof(percentage))]
        private bool reduceFromEach = default;
        [OdinSerialize]
        private bool percentage = default;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            return new ReduceDamageFilter(damageTypes, (amountToReduceBy == null ? 0 : (int)amountToReduceBy.Build(owner, target, arguments)), reduceFromEach, percentage);
        }
    }
}
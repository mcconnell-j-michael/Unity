using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class ConvertDamageFilterBuilder : I_FilterBuilder
    {

        [HorizontalGroup(nameof(ConvertDamageFilter))]

        [VerticalGroup(nameof(ConvertDamageFilter) + "/" + nameof(fromDamageTypes))]
        //[OdinSerialize, Title("From")]
        //[LabelText(StaticUtilities.BEFORE_TYPE + nameof(DamageType) + StaticUtilities.AFTER_TYPE + nameof(fromDamageTypes) + StaticUtilities.END)]
        //[EnumSODropdown, AutoPopulate]
        //private List<DamageType> fromDamageTypes = default;
        [OdinSerialize, Title("From"), AutoPopulate, EnumSODropdown]
        private List<DamageType> fromDamageTypes = default;
        [VerticalGroup(nameof(ConvertDamageFilter) + "/" + nameof(toDamageType))]
        [OdinSerialize, HideLabel, Title("To"), EnumSODropdown]
        private DamageType toDamageType = default;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            return new ConvertDamageFilter(fromDamageTypes == null ? new() : fromDamageTypes, toDamageType);
        }
    }
}
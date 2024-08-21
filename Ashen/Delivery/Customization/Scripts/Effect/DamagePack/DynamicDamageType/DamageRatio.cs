using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    /**
     * The DamageRatio holds a DamageType and a ratio that represents the percentatge of
     * the total damage that it will return
     **/
     [HideLabel, Title("Damage Ratio"), HideReferenceObjectPicker, AutoPopulate]
    public class DamageRatio
    {
        [HorizontalGroup(nameof(DamageRatio))]

        [VerticalGroup(nameof(DamageRatio) + "/" + nameof(damageType))]
        [EnumSODropdown, HideLabel, Required]
        public DamageType damageType;

        [VerticalGroup(nameof(DamageRatio) + "/" + nameof(ratio))]
        [Range(0f,1f), LabelWidth(40)]
        public float ratio;

        public DamageRatio(){}

        public DamageRatio(DamageType damageType, float ratio)
        {
            this.damageType = damageType;
            this.ratio = ratio;
        }
    }
}

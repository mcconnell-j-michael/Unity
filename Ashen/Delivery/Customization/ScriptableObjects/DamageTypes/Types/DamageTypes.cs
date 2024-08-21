using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * See DamageType
     **/
    [CreateAssetMenu(fileName = nameof(DamageTypes), menuName = "Custom/Enums/" + nameof(DamageTypes) + "/Types")]
    public class DamageTypes : A_EnumList<DamageType, DamageTypes>
    {
        public DamageType NORMAL;
    }
}
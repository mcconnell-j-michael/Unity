using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * The SimpleDamgeType holds a single DamageRatio for one DamageType at 100%
     **/
    public class SimpleDamageType : I_DynamicDamageType
    {
        private List<DamageRatio> damages;
        [OdinSerialize, HideLabel, EnumSODropdown]
        private DamageType damageType;

        public SimpleDamageType() { }
        public SimpleDamageType(DamageType damageType)
        {
            this.damageType = damageType;
        }

        public List<DamageRatio> GetDamageTypes(ToolManager target)
        {
            if (damages == null)
            {
                damages = new List<DamageRatio>();
                DamageRatio dr = new DamageRatio(damageType, 1.0f);
                damages.Add(dr);
            }
            return damages;
        }

        public override string ToString()
        {
            return damageType.name;
        }
    }
}

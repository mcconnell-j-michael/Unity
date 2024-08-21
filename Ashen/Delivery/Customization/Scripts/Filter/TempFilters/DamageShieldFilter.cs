using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * This TempFilter will automatically be removed after it blocks
     * a set amount of damage
     **/
    [Serializable]
    public class DamageShieldFilter : A_TempFilter
    {
        private List<DamageType> damageTypes = default;
        private int shieldTotal;

        public DamageShieldFilter()
        { }

        public DamageShieldFilter(List<DamageType> damageTypes, int shieldTotal)
        {
            this.damageTypes = damageTypes;
            this.shieldTotal = shieldTotal;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            int totalReduced = FilterUtility.ReduceDamage(damageTypes, deliveryResult, -1, shieldTotal, false);
            shieldTotal -= totalReduced;
            return totalReduced > 0;
        }

        public override bool Enabled()
        {
            return shieldTotal > 0 && base.Enabled();
        }

        public DamageShieldFilter(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            damageTypes = StaticUtilities.LoadList(info, nameof(damageTypes), (string name) =>
            {
                return DamageTypes.Instance[info.GetInt32(name)];
            });
            shieldTotal = info.GetInt32(nameof(shieldTotal));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectDataInternal(info, context);
            StaticUtilities.SaveList(info, nameof(damageTypes), damageTypes, (string name, DamageType damageType) =>
            {
                info.AddValue(name, (int)damageType);
            });
            info.AddValue(nameof(shieldTotal), shieldTotal);
        }
    }
}

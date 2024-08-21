using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * This is a filter that will reduce the damage of any DamageType inside of the passed in DamageContainer.
     **/
    [Serializable]
    public class ReduceDamageFilter : A_BaseFilter
    {
        private List<DamageType> damageTypes;
        private int amountToReduceBy;
        private bool reduceFromEach;
        private bool percentage;

        public ReduceDamageFilter() { }

        public ReduceDamageFilter(List<DamageType> damageTypes, int amountToReduceBy, bool reduceFromEach, bool percentage)
        {
            this.damageTypes = damageTypes;
            this.amountToReduceBy = amountToReduceBy;
            this.reduceFromEach = reduceFromEach;
            this.percentage = percentage;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            if (!percentage)
            {
                return FilterUtility.ReduceDamage(damageTypes, deliveryResult, amountToReduceBy, -1, reduceFromEach) > 0;
            }
            bool reduced = false;
            DamageResult dr = deliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            foreach (DamageType damageType in DamageTypes.Instance)
            {
                int value = dr.GetDamage(damageType);
                if (value <= 0)
                {
                    continue;
                }
                int reduceBy = (int)(value / 2f);
                if (reduceBy == 0)
                {
                    continue;
                }
                dr.AddDamage(damageType, -reduceBy);
                reduced = true;
            }
            return reduced;
        }

        public ReduceDamageFilter(SerializationInfo info, StreamingContext context)
        {
            damageTypes = StaticUtilities.LoadList(info, nameof(damageTypes), (string name) =>
            {
                return DamageTypes.Instance[info.GetInt32(name)];
            });
            amountToReduceBy = info.GetInt32(nameof(amountToReduceBy));
            reduceFromEach = info.GetBoolean(nameof(reduceFromEach));
            percentage = info.GetBoolean(nameof(percentage));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(damageTypes), damageTypes, (string name, DamageType damageType) =>
            {
                info.AddValue(name, (int)damageType);
            });
            info.AddValue(nameof(amountToReduceBy), amountToReduceBy);
            info.AddValue(nameof(percentage), percentage);
            info.AddValue(nameof(reduceFromEach), reduceFromEach);
        }
    }
}
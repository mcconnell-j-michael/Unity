using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ConvertDamageFilter : A_BaseFilter
    {
        private bool[] fromDamageTypes = default;
        private DamageType toDamageType = default;

        public ConvertDamageFilter(List<DamageType> fromDamageTypes, DamageType toDamageType)
        {
            this.fromDamageTypes = new bool[DamageTypes.Count];
            foreach (DamageType damage in fromDamageTypes)
            {
                this.fromDamageTypes[(int)damage] = true;
            }
            this.toDamageType = toDamageType;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            DamageResult damageResult = deliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            int total = 0;
            foreach (DamageType damageType in DamageTypes.Instance)
            {
                if (fromDamageTypes[(int)damageType] && damageResult.GetDamage(damageType) > 0)
                {
                    total += damageResult.GetDamage(damageType);
                    damageResult.ResetDamage(damageType);
                }
            }
            damageResult.AddDamage(toDamageType, total);
            return total > 0;
        }

        public ConvertDamageFilter(SerializationInfo info, StreamingContext context)
        {
            fromDamageTypes = (bool[])info.GetValue(nameof(fromDamageTypes), typeof(bool[]));
            toDamageType = DamageTypes.Instance[info.GetInt32(nameof(toDamageType))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(fromDamageTypes), fromDamageTypes);
            info.AddValue(nameof(toDamageType), (int)toDamageType);
        }
    }
}
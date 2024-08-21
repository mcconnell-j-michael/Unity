using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    public class CollectiveDamagePack : I_Effect, ISerializable
    {
        private List<DamageType> damageTypes;
        private float value;

        public CollectiveDamagePack() { }
        public CollectiveDamagePack(List<DamageType> damageTypes, float value)
        {
            this.damageTypes = damageTypes;
            this.value = value;
        }

        public int GetAmount(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return (int)value;
        }

        public void Apply(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            int total = GetAmount(dOwner, dTarget, deliveryArguments);
            if (total == 0)
            {
                return;
            }
            DamageResult deliveryResult = targetDeliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            deliveryResult.AddDamage(deliveryResult.combineInto, total);
            foreach (DamageType dt in damageTypes)
            {
                deliveryResult.EnableDamageType(dt);
            }
        }

        protected CollectiveDamagePack(SerializationInfo info, StreamingContext context)
        {
            damageTypes = StaticUtilities.LoadList(info, nameof(damageTypes), (string name) =>
            {
                return DamageTypes.Instance[info.GetInt32(name)];
            });
            value = (float)info.GetValue(nameof(value), typeof(float));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(damageTypes), damageTypes, (string name, DamageType damageType) =>
            {
                info.AddValue(name, (int)damageType);
            });
            info.AddValue(nameof(value), value);
        }
    }
}
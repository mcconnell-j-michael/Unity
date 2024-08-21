using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * A DamagePack is an Effect that contains a DynamicDamageType and a DynamicNumber
     * The DynamicDamageType is used to determine the types of damage to do and how to 
     * distribute the results of the DynamicNumber
     **/
    [Serializable]
    public class DamagePack : I_Effect, ISerializable
    {
        private DamageType damageType;
        private float value;

        public DamagePack() { }
        public DamagePack(DamageType damageType, float value)
        {
            this.damageType = damageType;
            this.value = value;
        }

        public int GetAmount(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return (int)value;
        }

        public void Apply(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            int total = GetAmount(dOwner, dTarget, deliveryArguments);
            ToolManager target = (dTarget as DeliveryTool).toolManager;
            DamageResult deliveryResult = targetDeliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            deliveryResult.AddDamage(damageType, total);
            if (total != 0)
            {
                targetDeliveryResult.empty = false;
            }
        }

        protected DamagePack(SerializationInfo info, StreamingContext context)
        {
            value = (float)info.GetValue(nameof(value), typeof(float));
            damageType = DamageTypes.Instance[info.GetInt32(nameof(damageType))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(value), value);
            info.AddValue(nameof(damageType), (int)damageType);
        }
    }
}
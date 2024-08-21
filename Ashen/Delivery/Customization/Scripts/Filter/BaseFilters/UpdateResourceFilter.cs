using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class UpdateResourceFilter : A_BaseFilter
    {
        private ResourceValue resourceValue;
        private float amount;
        private bool increase;
        private bool percentageOfDamage;
        private bool target;

        public UpdateResourceFilter(ResourceValue resourceValue, float amount, bool increase, bool percentageOfDamage, bool target)
        {
            this.resourceValue = resourceValue;
            this.amount = amount;
            this.increase = increase;
            this.percentageOfDamage = percentageOfDamage;
            this.target = target;
        }

        public override bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult)
        {
            ToolManager primary = this.target ? (target as DeliveryTool).toolManager : (owner as DeliveryTool).toolManager;
            int total = 0;
            if (percentageOfDamage)
            {
                DamageResult result = deliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
                int totalDamage = 0;
                foreach (DamageType dt in DamageTypes.Instance)
                {
                    totalDamage += result.GetDamage(dt);
                }
                total = (int)(this.amount * totalDamage);
            }
            else
            {
                total = (int)(this.amount);
            }
            if (total <= 0)
            {
                return false;
            }
            ResourceValueTool rvTool = primary.Get<ResourceValueTool>();
            if (this.increase)
            {
                rvTool.RemoveAmount(this.resourceValue, total);
            }
            else
            {
                rvTool.ApplyAmount(this.resourceValue, total);
            }
            return true;
        }

        public UpdateResourceFilter(SerializationInfo info, StreamingContext context)
        {
            resourceValue = ResourceValues.Instance[info.GetInt32(nameof(resourceValue))];
            amount = (float)info.GetValue(nameof(amount), typeof(float));
            increase = info.GetBoolean(nameof(increase));
            percentageOfDamage = info.GetBoolean(nameof(percentageOfDamage));
            target = info.GetBoolean(nameof(target));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(resourceValue), (int)resourceValue);
            info.AddValue(nameof(amount), amount);
            info.AddValue(nameof(increase), increase);
            info.AddValue(nameof(percentageOfDamage), percentageOfDamage);
            info.AddValue(nameof(target), target);
        }
    }
}
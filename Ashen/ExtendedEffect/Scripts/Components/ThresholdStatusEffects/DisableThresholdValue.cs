using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class DisableThresholdValue : A_SimpleComponent
    {
        [EnumSODropdown, HideLabel, Title("Damage Type")]
        public ResourceValue resourceValue;

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deilveryTool = dse.target as DeliveryTool;
            ResourceValueTool resourceValueTool = deilveryTool.toolManager.Get<ResourceValueTool>();
            if (resourceValueTool)
            {
                resourceValueTool.DisableThresholdValue(resourceValue);
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deilveryTool = dse.target as DeliveryTool;
            ResourceValueTool resourceValueTool = deilveryTool.toolManager.Get<ResourceValueTool>();
            if (resourceValueTool)
            {
                resourceValueTool.EnableThresholdValue(resourceValue);
            }
        }

        public DisableThresholdValue(SerializationInfo info, StreamingContext context)
        {
            resourceValue = ResourceValues.Instance[info.GetInt32(nameof(resourceValue))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(resourceValue), (int)resourceValue);
        }
    }
}
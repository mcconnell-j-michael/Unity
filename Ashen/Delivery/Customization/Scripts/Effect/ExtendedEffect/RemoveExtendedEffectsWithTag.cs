using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class RemoveExtendedEffectsWithTag : I_Effect
    {
        private List<ExtendedEffectTag> tags;

        public RemoveExtendedEffectsWithTag(List<ExtendedEffectTag> tags)
        {
            this.tags = tags;
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool tDeliveryTool = target as DeliveryTool;
            if (tDeliveryTool)
            {
                StatusTool tStatusTool = tDeliveryTool.toolManager.Get<StatusTool>();
                if (tStatusTool)
                {
                    foreach (ExtendedEffectTag tag in tags)
                    {
                        tStatusTool.DisableAllStatusEffectsWithTag(tag);
                    }
                }
            }
        }

        protected RemoveExtendedEffectsWithTag(SerializationInfo info, StreamingContext context)
        {
            tags = StaticUtilities.LoadList(info, nameof(tags), (string name) =>
            {
                return ExtendedEffectTags.Instance[info.GetInt32(name)];
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(tags), tags, (string name, ExtendedEffectTag tag) =>
            {
                info.AddValue(name, (int)tag);
            });
        }
    }
}
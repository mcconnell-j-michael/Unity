using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class RemoveExtendedEffectsWithTagBuilder : I_EffectBuilder
    {
        [AutoPopulate, EnumSODropdown]
        public List<ExtendedEffectTag> tags;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new RemoveExtendedEffectsWithTag(tags);
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "Disable StatusEffects with tags: [";
            if (tags == null)
            {
                return visualization + "]";
            }
            int count = 0;
            foreach (ExtendedEffectTag tag in tags)
            {
                if (count != 0)
                {
                    visualization += ", ";
                }
                visualization += tag.ToString();
                count++;
            }
            return visualization + "]";
        }

        public RemoveExtendedEffectsWithTagBuilder(SerializationInfo info, StreamingContext context)
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
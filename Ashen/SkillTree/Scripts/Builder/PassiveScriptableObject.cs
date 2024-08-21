using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Ashen.SkillTree
{
    public class PassiveScriptableObject : SerializedScriptableObject
    {
        [OdinSerialize, Hide]
        public SkillNodeEffectBuilder statusEffect;

        public I_ExtendedEffect Clone(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks)
        {
            return statusEffect.Build(owner, target, deliveryArgumentPacks);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
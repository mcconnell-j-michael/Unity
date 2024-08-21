using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    public class RetargetEffectBuilder : I_EffectBuilder
    {
        [OdinSerialize]
        private AbilityTag[] abilityTags;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new RetargetEffect(abilityTags);
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Retarget effect to owner if ability has any tag of [" + abilityTags.ToString() + "]";
            return vis;
        }
    }
}
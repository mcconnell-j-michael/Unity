using Ashen.DeliverySystem;
using Sirenix.Serialization;

namespace Ashen.ExtendedEffectSystem
{
    public class DerivedAttributeListenerComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private DerivedAttribute listenOn;

        [OdinSerialize, AutoPopulate(instance = typeof(ExtendedEffectBuilder)), HideWithoutAutoPopulate]
        private I_ExtendedEffectBuilder statusEffect;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new DerivedAttributeListenerComponent(listenOn, statusEffect);
        }
    }
}

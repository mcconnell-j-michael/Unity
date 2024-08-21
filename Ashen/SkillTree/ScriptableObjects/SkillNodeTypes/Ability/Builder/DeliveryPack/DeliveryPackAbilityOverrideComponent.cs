using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;

namespace Ashen.SkillTree
{
    public class DeliveryPackAbilityOverrideComponent : I_AbilityOverrideComponent, I_SubAbilityOverrideComponent
    {
        [Hide, Title("Delivery Packs")]
        public DeliveryPackBuilder deliveryPack;

        public void Override(AbilityAction abilityAction)
        {
            AbilityDeliveryPackProcessor deliveryProcessor = abilityAction.Get<AbilityDeliveryPackProcessor>();
            if (deliveryPack.deliveryPack != null)
            {
                deliveryProcessor.deliveryPack = deliveryPack;
            }
        }
    }
}
using Ashen.DeliverySystem;
using Ashen.ItemSystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityDeliveryPacks : I_AbilityBuilder, I_SubAbilityBuilder, I_ItemBuilder
    {
        [Title("Attribute"), HideLabel]
        public TargetAttribute attributeEffects;
        [Hide, Title("Delivery Packs")]
        public DeliveryPackBuilder deliveryPack;

        public I_AbilityProcessor Build(Ability ability)
        {
            AbilityDeliveryPackProcessor processor = new AbilityDeliveryPackProcessor
            {
                targetAttribute = attributeEffects,
                deliveryPack = deliveryPack
            };
            return processor;
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new AbilityDeliveryPacks();
        }

        public string GetTabName()
        {
            return "Delivery";
        }
    }
}
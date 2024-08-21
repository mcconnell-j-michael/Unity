using Ashen.DeliverySystem;
using Ashen.ItemSystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityHitChance : I_AbilityBuilder, I_ItemBuilder
    {
        [ToggleGroup(nameof(canMiss))]
        public bool canMiss;
        [ToggleGroup(nameof(canMiss))]
        public I_DeliveryValue hitChanceValue;

        [ToggleGroup(nameof(canCrit))]
        public bool canCrit;
        [ToggleGroup(nameof(canCrit))]
        public I_DeliveryValue critChanceValue;

        public I_AbilityProcessor Build(Ability ability)
        {
            AbilityHitChanceProcessor processor = new AbilityHitChanceProcessor();
            if (canMiss)
            {
                processor.hitChance = hitChanceValue;
            }
            if (canCrit)
            {
                processor.critChance = critChanceValue;
            }
            return processor;
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new AbilityHitChance();
        }

        public string GetTabName()
        {
            return "Hit Chance";
        }
    }
}
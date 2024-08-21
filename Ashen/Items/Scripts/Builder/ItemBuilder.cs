using Ashen.AbilitySystem;
using Sirenix.OdinInspector;

namespace Ashen.ItemSystem
{
    public class ItemBuilder : A_AbilityBuilder<I_ItemBuilder>
    {
        [HideLabel, Title("Name"), PropertyOrder(-1)]
        public string name;

        public Ability Build()
        {
            Ability ability = new()
            {
                name = name
            };
            AbilityAction abilityAction = new()
            {
                sourceAbility = ability,
                abilityArguments = new DeliveryArgumentPacks()
            };

            ability.abilityAction = abilityAction;

            FillAbilityAction(ability, abilityAction);

            return ability;
        }
    }
}
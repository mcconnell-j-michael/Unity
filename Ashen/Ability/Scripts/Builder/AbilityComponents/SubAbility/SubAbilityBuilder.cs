namespace Ashen.AbilitySystem
{
    public class SubAbilityBuilder : A_AbilityBuilder<I_SubAbilityBuilder>
    {
        public AbilityAction Build(Ability ability)
        {
            AbilityAction action = new()
            {
                sourceAbility = ability,
                abilityArguments = new DeliveryArgumentPacks()
            };

            FillAbilityAction(ability, action);

            return action;
        }
    }
}
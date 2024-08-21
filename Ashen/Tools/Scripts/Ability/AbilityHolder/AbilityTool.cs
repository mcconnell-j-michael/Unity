using Ashen.AbilitySystem;

namespace Ashen.ToolSystem
{
    public class AbilityTool : A_ConfigurableTool<AbilityTool, AbilityHolderConfiguration>
    {
        private Ability attackAbility;
        public Ability AttackAbility { get { return attackAbility; } }

        private Ability defendAbility;
        public Ability DefendAbility { get { return defendAbility; } }

        private AbilityHolder abilityHolder;
        public AbilityHolder AbilityHolder { get { return abilityHolder; } }

        public override void Initialize()
        {
            base.Initialize();
            abilityHolder = new AbilityHolder();
            abilityHolder.Initialize();
            foreach (AbilitySO abilitySO in Config.DefaultAbilities)
            {
                abilityHolder.GrantAbility(abilitySO.builder.name, abilitySO.builder.Build());
            }
            attackAbility = Config.AttackAbility.builder.Build();
            defendAbility = Config.DefendAbility.builder.Build();
        }
    }
}
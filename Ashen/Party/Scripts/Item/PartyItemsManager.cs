using Ashen.AbilitySystem;
using Ashen.ItemSystem;
using Ashen.ToolSystem;

namespace Ashen.PartySystem
{
    public class PartyItemsManager : A_ConfigurableTool<PartyItemsManager, PartyItemConfiguration>
    {
        private AbilityHolder abilityHolder;
        public AbilityHolder AbilityHolder { get { return abilityHolder; } }

        public override void Initialize()
        {
            abilityHolder = new AbilityHolder();
            abilityHolder.Initialize();
            foreach (ItemSO itemSO in Config.DefaultItems)
            {
                abilityHolder.GrantAbility(itemSO.itemBuilder.name, itemSO.itemBuilder.Build());
            }
        }
    }
}
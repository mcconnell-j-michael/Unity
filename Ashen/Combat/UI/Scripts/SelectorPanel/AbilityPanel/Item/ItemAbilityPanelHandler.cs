using Ashen.AbilitySystem;
using Ashen.ItemSystem;
using Ashen.PartySystem;

namespace Ashen.CombatSystem
{
    public class ItemAbilityPanelHandler : A_AbilityPanelHandler<ItemAbilityPanelHandler, ItemAbilitySelector>
    {
        public override void OnLoad(Ability ability, ItemAbilitySelector selector)
        {
            PlayerPartyManager manager = PlayerPartyHolder.Instance.partyManager;
            PartyResourceTracker resourceTracker = manager.partyToolManager.Get<PartyResourceTracker>();
            ItemRuneFragmentCostProcessor costProcessor = ability.abilityAction.Get<ItemRuneFragmentCostProcessor>();
            selector.Initialize();
            if (costProcessor == null)
            {
                //TODO
                selector.Valid = true;
                return;
            }
            bool valid = true;
            foreach (PartyResource resource in PartyResources.Instance)
            {
                int cost = costProcessor.costMap[(int)resource];
                int available = resourceTracker.GetReservedTotal(resource);
                if (cost == 0)
                {
                    selector.containers[(int)resource].gameObject.SetActive(false);
                    continue;
                }
                selector.containers[(int)resource].costText.text = cost.ToString();
                valid = valid && cost <= available;
            }
            selector.Valid = valid;
        }

        protected override void RegisterSelector(ItemAbilitySelector abilitySelector)
        {
            abilitySelector.handler = this;
        }
    }
}
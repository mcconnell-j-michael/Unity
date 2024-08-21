using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.ItemSystem;
using Ashen.PartySystem;
using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class ItemChooseAbilityPanel : A_ChooseAbilityPanel<ItemAbilityPanelHandler, ItemAbilitySelector>
    {
        public ItemChooseAbilityPanel(I_GameState previousState) : base(previousState) { }

        public override IEnumerator GetPanelHandler()
        {
            AbilityHolder abilityHolder = PlayerPartyHolder.Instance.partyManager.partyToolManager.Get<PartyItemsManager>().AbilityHolder;

            if (abilityHolder.GetCount() == 0)
            {
                yield break;
            }

            ItemAbilityPanelHandler itemPanel = ItemAbilityPanelHandler.Instance;
            yield return itemPanel.LoadAbilities(abilityHolder);
            panelHandler = itemPanel;
        }

        public override void OnSubmitInternal()
        {
            base.OnSubmitInternal();
            ItemRuneFragmentCostProcessor processor = previousAbility.abilityAction.Get<ItemRuneFragmentCostProcessor>();
            PartyResourceCharacterCommitment commitment = new PartyResourceCharacterCommitment(processor.costMap,
                PlayerPartyHolder.Instance.partyManager.GetComponent<PartyResourceTracker>(), PlayerInputState.Instance.GetActionCount());
            CombatTool ct = PlayerInputState.Instance.currentlySelected.Get<CombatTool>();
            ct.AddCommitment(commitment);
        }

        public override void Reset()
        {
            CombatTool ct = PlayerInputState.Instance.currentlySelected.Get<CombatTool>();
            ct.RemoveLastCommitment();
        }
    }
}
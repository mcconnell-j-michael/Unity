using Ashen.PartySystem;
using System;

namespace Ashen.CombatSystem
{
    public class ItemAbilitySelector : A_AbilitySelector
    {
        [NonSerialized]
        public ItemAbilityPanelHandler handler;
        [NonSerialized]
        public ItemCostUiContainer[] containers;

        public void Initialize()
        {
            this.containers = new ItemCostUiContainer[PartyResources.Count];
            ItemCostUiContainer[] containers = GetComponentsInChildren<ItemCostUiContainer>(true);
            foreach (ItemCostUiContainer container in containers)
            {
                this.containers[(int)container.resource] = container;
            }
        }

        public override void OnSetValid(bool valid)
        {
        }
    }
}
using Ashen.AbilitySystem;
using Ashen.PartySystem;
using Ashen.ToolSystem;

namespace Ashen.ItemSystem
{
    public class ItemRuneFragmentCostProcessor : A_AbilityProcessor
    {
        public int[] costMap;

        public override void OnExecute(ToolManager toolManager, DeliveryArgumentPacks arguments)
        {
            PartyResourceTracker resourceTracker = PlayerPartyHolder.Instance.partyManager.partyToolManager.Get<PartyResourceTracker>();
            foreach (PartyResource resource in PartyResources.Instance)
            {
                if (costMap[(int)resource] != 0)
                {
                    resourceTracker.RemoveResource(resource, costMap[(int)resource]);
                }
            }
        }
    }
}
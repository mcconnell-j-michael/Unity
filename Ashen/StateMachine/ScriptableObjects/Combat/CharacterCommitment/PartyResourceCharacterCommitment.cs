using Ashen.PartySystem;
using Ashen.ToolSystem;

namespace Ashen.StateMachineSystem
{
    public class PartyResourceCharacterCommitment : A_CharacterCommitment
    {
        private int[] partyResources;
        private PartyResourceTracker tracker;

        public PartyResourceCharacterCommitment(int[] partyResources, PartyResourceTracker tracker, int actionCount) : base(actionCount)
        {
            this.partyResources = new int[PartyResources.Count];
            foreach (PartyResource resource in PartyResources.Instance)
            {
                this.partyResources[(int)resource] = partyResources[(int)resource];
            }
            this.tracker = tracker;
        }

        protected override void FinalizeInternal(ToolManager toolManager)
        {
        }

        protected override void OnCommitInternal(ToolManager toolManager)
        {
            if (!tracker)
            {
                return;
            }
            foreach (PartyResource resource in PartyResources.Instance)
            {
                if (partyResources[(int)resource] != 0)
                {
                    tracker.RemoveReservedResource(resource, partyResources[(int)resource]);
                }
            }
        }

        protected override void RollbackInternal(ToolManager toolManager)
        {
            if (!tracker)
            {
                return;
            }
            foreach (PartyResource resource in PartyResources.Instance)
            {
                if (partyResources[(int)resource] != 0)
                {
                    tracker.AddReservedResource(resource, partyResources[(int)resource]);
                }
            }
        }
    }
}
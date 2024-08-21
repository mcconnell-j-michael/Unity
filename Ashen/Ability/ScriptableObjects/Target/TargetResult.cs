using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public struct TargetResult
    {
        public ToolManager source;
        public ToolManager target;
        public A_PartyManager sourceParty;
        public A_PartyManager targetParty;
        public TargetCategory category;
    }
}
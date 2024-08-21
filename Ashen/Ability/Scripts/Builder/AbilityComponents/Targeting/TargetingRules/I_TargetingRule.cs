using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public interface I_TargetingRule
    {
        bool IsValidTarget(ToolManager source, ToolManager target, PartyPosition position);
    }
}
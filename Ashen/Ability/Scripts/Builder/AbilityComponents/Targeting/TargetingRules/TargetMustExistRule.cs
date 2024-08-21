using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public class TargetMustExistRule : I_TargetingRule
    {
        public bool IsValidTarget(ToolManager source, ToolManager target, PartyPosition position)
        {
            if (!target)
            {
                return false;
            }
            return true;
        }
    }
}
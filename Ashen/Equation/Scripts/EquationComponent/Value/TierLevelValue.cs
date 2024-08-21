using Ashen.AbilitySystem;
using Ashen.ToolSystem;

namespace Ashen.EquationSystem
{
    public class TierLevelValue : A_CacheableValue<ShiftableTierLevelTool, AbilityTag, int>
    {
        protected override ShiftableTierLevelTool GetCachingTool(ToolManager toolManager)
        {
            return toolManager.Get<ShiftableTierLevelTool>();
        }

        protected override AbilityTag GetEnumFromIndex(int index)
        {
            return AbilityTags.GetEnum(index);
        }
    }
}
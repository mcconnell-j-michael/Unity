using Ashen.ToolSystem;

namespace Ashen.CombatSystem
{
    public class CombatResourceBarManager : ResourceBarManager
    {
        protected override void InternalRegisterToolManager(ToolManager toolManager)
        {
            base.InternalRegisterToolManager(toolManager);
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            rvTool.RegisterThresholdMetListener(resourceValue, this);
        }
    }
}
using Ashen.ToolSystem;

namespace Ashen.CombatSystem
{
    public interface I_ResourceManagerUI
    {
        void RegisterToolManager(ToolManager toolManager);
        void UnregisterToolManager();
    }
}
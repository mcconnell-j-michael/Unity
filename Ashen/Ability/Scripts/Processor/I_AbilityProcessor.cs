using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public interface I_AbilityProcessor
    {
        public void OnLoad(DeliveryArgumentPacks arguments);
        public void OnExecute(ToolManager toolManager, DeliveryArgumentPacks arguments);
    }
}
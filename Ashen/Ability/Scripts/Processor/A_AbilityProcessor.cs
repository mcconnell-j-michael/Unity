using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public abstract class A_AbilityProcessor : I_AbilityProcessor
    {
        public virtual void OnLoad(DeliveryArgumentPacks arguments)
        { }
        public virtual void OnExecute(ToolManager toolManager, DeliveryArgumentPacks arguments)
        { }
    }
}
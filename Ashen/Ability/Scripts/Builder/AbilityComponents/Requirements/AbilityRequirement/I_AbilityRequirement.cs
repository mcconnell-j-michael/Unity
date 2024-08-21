using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public interface I_AbilityRequirement
    {
        bool IsValid(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments);
    }
}
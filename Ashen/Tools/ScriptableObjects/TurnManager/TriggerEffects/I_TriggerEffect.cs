using Ashen.DeliverySystem;

namespace Ashen.ToolSystem
{
    public interface I_TriggerEffect
    {
        void OnTriggerEffect(ToolManager toolManager, ExtendedEffectTrigger extendedEffectTrigger);
    }
}
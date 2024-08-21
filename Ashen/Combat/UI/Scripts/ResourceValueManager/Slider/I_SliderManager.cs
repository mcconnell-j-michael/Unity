using Ashen.DeliverySystem;
using Ashen.ToolSystem;

namespace Ashen.CombatSystem
{
    public interface I_SliderManager
    {
        public void UpdateFill(ToolManager toolManager, ThresholdEventValue value);
        public void ResetFill();
    }
}
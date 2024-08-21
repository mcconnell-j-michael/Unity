using Ashen.DeliverySystem;
using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public class AbilityHitChanceProcessor : A_AbilityProcessor
    {
        public I_DeliveryValue hitChance;
        public I_DeliveryValue critChance;

        public bool CanMiss()
        {
            return hitChance != null;
        }

        public bool CanCrit()
        {
            return critChance != null;
        }

        public float GetBonusHitChance(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments)
        {
            if (CanMiss())
            {
                DeliveryTool dTool = toolManager.Get<DeliveryTool>();
                return hitChance.Build(dTool, dTool, deliveryArguments);
            }
            return 0;
        }

        public float GetBonusCritChance(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments)
        {
            if (CanCrit())
            {
                DeliveryTool dTool = toolManager.Get<DeliveryTool>();
                return critChance.Build(dTool, dTool, deliveryArguments);
            }
            return 0;
        }
    }
}
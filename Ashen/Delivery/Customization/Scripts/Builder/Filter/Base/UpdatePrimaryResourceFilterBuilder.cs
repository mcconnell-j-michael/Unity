using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Ashen.DeliverySystem
{
    public class UpdatePrimaryResourceFilterBuilder : I_FilterBuilder
    {
        [HideLabel, EnumToggleButtons]
        public PrimaryResourceFilterType type;
        [HideLabel, EnumToggleButtons, Title("Target?")]
        public TargetChoice targetChoice;
        [OdinSerialize, Hide]
        private I_DeliveryValue value;

        public I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            float val = value.Build(owner, target, arguments);
            ToolManager toolManager = targetChoice == TargetChoice.Owner ? (owner as DeliveryTool).toolManager : (target as DeliveryTool).toolManager;
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            return new UpdateResourceFilter(ResourceValues.Instance.ABILITY_RESOURCE, val, val > 0, type == PrimaryResourceFilterType.Percentage, targetChoice == TargetChoice.Target);
        }
    }

    public enum PrimaryResourceFilterType
    {
        Percentage, Flat
    }
}
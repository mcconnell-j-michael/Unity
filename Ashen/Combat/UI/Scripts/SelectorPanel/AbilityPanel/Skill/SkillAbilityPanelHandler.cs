using Ashen.AbilitySystem;
using Ashen.ObjectPoolSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class SkillAbilityPanelHandler : A_AbilityPanelHandler<SkillAbilityPanelHandler, SkillAbilitySelector>
    {
        private ResourceValueTool resourceValueTool;
        private ToolManager toolManager;

        [HideLabel, Title("valid cost")]
        public Color validCost;
        [HideLabel, Title("invalid cost")]
        public Color invalidCost;

        public void RegisterToolManager(ToolManager toolManager)
        {
            resourceValueTool = toolManager.Get<ResourceValueTool>();
            this.toolManager = toolManager;
        }

        protected override void RegisterSelector(SkillAbilitySelector abilitySelector)
        {
            abilitySelector.handler = this;
        }

        public override void OnLoad(Ability ability, SkillAbilitySelector abilitySelector)
        {
            DeliveryArgumentPacks deliveryArguments = AGenericPool<DeliveryArgumentPacks>.Get();
            AbilityRequirementsProcessor requirementsProcessor = ability.abilityAction.Get<AbilityRequirementsProcessor>();
            int totalCost = requirementsProcessor.GetResourceChange(ResourceValues.Instance.ABILITY_RESOURCE, resourceValueTool.toolManager, ability.abilityAction.abilityArguments);
            if (totalCost == 0)
            {
                abilitySelector.skillCost.text = "";
            }
            else if (totalCost < 0)
            {
                abilitySelector.skillCost.text = "^" + (-totalCost);
            }
            else
            {
                abilitySelector.skillCost.text = totalCost.ToString();
            }
            abilitySelector.Valid = requirementsProcessor.IsValid(resourceValueTool.toolManager, deliveryArguments);
            TargetingProcessor targetingProcessor = ability.abilityAction.Get<TargetingProcessor>();
            ShiftableTierLevelTool stlt = toolManager.Get<ShiftableTierLevelTool>();
            abilitySelector.tierLevelsManager.SetTier(stlt.CalculateTierLevel(targetingProcessor.GetAbilityTags(toolManager)));
            AGenericPool<DeliveryArgumentPacks>.Release(deliveryArguments);
        }
    }
}
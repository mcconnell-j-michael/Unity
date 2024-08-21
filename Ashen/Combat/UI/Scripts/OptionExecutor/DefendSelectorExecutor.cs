using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using Ashen.UISystem;
using System;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class DefendSelectorExecutor : MonoBehaviour, I_OptionExecutor
    {
        public CombatOptionUI combatOption;

        public I_GameState GetGameState(I_GameState parentState)
        {
            AbilityTool abilityHolder = PlayerInputState.Instance.currentlySelected.Get<AbilityTool>();

            return new ChooseTargetCombat(parentState, abilityHolder.DefendAbility);
        }

        public void InitializeOption(ToolManager source)
        {
            AbilityTool abilityHolder = source.Get<AbilityTool>();
            ResourceValueTool rvt = source.Get<ResourceValueTool>();

            AbilityAction abilityAction = abilityHolder.DefendAbility.abilityAction;
            AbilityRequirementsProcessor requirementsProcessor = abilityAction.Get<AbilityRequirementsProcessor>();
            int actionPoints = Math.Max(
                requirementsProcessor.GetResourceChange(
                    ResourceValues.Instance.ACTION_POINT,
                    source,
                    abilityAction.abilityArguments,
                    ResourceChangeType.COST
                ),
            1);
            ThresholdEventValue value = rvt.GetValue(ResourceValues.Instance.ACTION_POINT);
            int currentValue = rvt.CalculateLimit(ResourceValues.Instance.ACTION_POINT, value.tempValues[(int)ThresholdValueTempCategories.Instance.PROMISED]);
            combatOption.Valid = currentValue >= actionPoints;
        }

        public void Selected(ToolManager source)
        {
            if (!combatOption.Valid)
            {
                return;
            }
            AbilityTool abilityHolder = source.Get<AbilityTool>();
            ResourceValueTool rvt = source.Get<ResourceValueTool>();

            AbilityAction abilityAction = abilityHolder.DefendAbility.abilityAction;
            AbilityRequirementsProcessor requirementsProcessor = abilityAction.Get<AbilityRequirementsProcessor>();
            int actionPoints = requirementsProcessor.GetResourceChange(
                ResourceValues.Instance.ACTION_POINT,
                source,
                abilityAction.abilityArguments,
                ResourceChangeType.COST
            );
            rvt.ApplyTempAmount(ResourceValues.Instance.ACTION_POINT, ThresholdValueTempCategories.Instance.PREVIEW, new TempValueContainer(actionPoints));
        }

        public void Deselected(ToolManager source)
        {
            ResourceValueTool rvt = source.Get<ResourceValueTool>();
            rvt.ClearTempValues(ResourceValues.Instance.ACTION_POINT, ThresholdValueTempCategories.Instance.PREVIEW);
        }
    }
}
using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System;
using System.Collections.Generic;

namespace Ashen.StateMachineSystem
{
    public class ActionCommitment : A_CharacterCommitment
    {

        private List<ActionProcessor> actionProcessors;
        private string name;
        private TempValueContainer[] valueContainers;
        private AbilityAction abilityAction;

        public ActionCommitment(List<ActionProcessor> processors, AbilityAction abilityAction, string name, int actionCount) : base(actionCount)
        {
            this.actionProcessors = processors;
            valueContainers = new TempValueContainer[ResourceValues.Count];
            this.abilityAction = abilityAction;
            this.name = name;
        }

        protected override void OnCommitInternal(ToolManager toolManager)
        {

            foreach (ActionProcessor actionProcessor in actionProcessors)
            {
                actionProcessor.SetActionCount(ActionCount);
                PlayerPartyHolder.Instance.partyManager.GetCurrentBattleContainer().AddPrimaryAction(actionProcessor);
            }
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            if (!rvTool)
            {
                return;
            }
            AbilityRequirementsProcessor requirementsProcessor = abilityAction.Get<AbilityRequirementsProcessor>();
            foreach (ResourceValue rv in ResourceValues.Instance)
            {
                int value = Math.Max(
                    requirementsProcessor.GetResourceChange(
                        rv,
                        toolManager,
                        abilityAction.abilityArguments,
                        ResourceChangeType.COST
                    ),
                0);
                valueContainers[(int)rv] = new TempValueContainer(value);
                rvTool.ApplyTempAmount(rv, ThresholdValueTempCategories.Instance.PROMISED, valueContainers[(int)rv]);
            }
            CombatTool ct = toolManager.Get<CombatTool>();
            ct.SetActionCost(ActionCount, -valueContainers[(int)ResourceValues.Instance.ACTION_POINT].TempValue, name);

        }

        protected override void RollbackInternal(ToolManager toolManager)
        {
            PlayerPartyHolder.Instance.partyManager.GetCurrentBattleContainer().ClearProcessors(toolManager, ActionCount);
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            if (!rvTool)
            {
                return;
            }
            foreach (ResourceValue rv in ResourceValues.Instance)
            {
                rvTool.DeleteTempValue(rv, ThresholdValueTempCategories.Instance.PROMISED, valueContainers[(int)rv]);
            }
            CombatTool ct = toolManager.Get<CombatTool>();
            ct.SetActionCost(ActionCount, 0);
        }

        protected override void FinalizeInternal(ToolManager toolManager)
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            if (!rvTool)
            {
                return;
            }
            foreach (ResourceValue rv in ResourceValues.Instance)
            {
                rvTool.DeleteTempValue(rv, ThresholdValueTempCategories.Instance.PROMISED, valueContainers[(int)rv]);
            }
        }

        protected override void UpdateActionCountInternal(int oldActionCount, int newActionCount)
        {
            foreach (ActionProcessor actionProcessor in actionProcessors)
            {
                actionProcessor.SetActionCount(newActionCount);
            }
        }
    }
}
using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.ObjectPoolSystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class ActionProcessor : A_CombatProcessor
    {
        public ToolManager source;
        public float speed;
        public AbilitySpeedCategory speedCategory;
        public AbilityAction sourceAbility;

        private A_PartyManager sourceParty;
        private A_PartyManager targetParty;
        private I_TargetHolder targetHolder;

        private ActionProcessorState currentState;

        private TriggerTool triggerTool;
        private DeliveryArgumentPacks arguments;

        public ActionProcessor(
            AbilityAction sourceAbility,
            ToolManager source,
            A_PartyManager sourceParty,
            A_PartyManager targetParty,
            I_TargetHolder targetHolder
        )
        {
            this.sourceParty = sourceParty;
            this.targetParty = targetParty;
            this.sourceAbility = sourceAbility;
            this.source = source;
            arguments = AGenericPool<DeliveryArgumentPacks>.Get();
            sourceAbility.abilityArguments.CopyInto(arguments);
            SpeedProcessor abilitySpeedProcessor = sourceAbility.Get<SpeedProcessor>();
            if (abilitySpeedProcessor != null)
            {
                if (abilitySpeedProcessor.GetSpeedCategory() == null)
                {
                    speedCategory = AbilitySpeedCategories.Instance.defaultSpeedCategory;
                }
                else
                {
                    speedCategory = abilitySpeedProcessor.GetSpeedCategory();
                }
                if (speedCategory.useSpeedCalculation)
                {
                    speed = Random.Range(1f, 10f) * Mathf.Max(0.1f, DerivedAttributes.GetEnum("Speed").equation.Calculate(source.Get<DeliveryTool>(), arguments));
                    if (abilitySpeedProcessor.GetSpeedFactor() != null)
                    {
                        speed *= abilitySpeedProcessor.GetSpeedFactor().Calculate(source.Get<DeliveryTool>(), arguments);
                    }
                }
            }
            this.targetHolder = targetHolder;
            currentState = ActionProcessorState.INITIAL;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public I_CombatProcessor GetNextExecutable()
        {
            return targetHolder.ResolveTarget();
        }

        public List<TargetResult> GetNextResults()
        {
            return targetHolder.FinalizeTargets();
        }

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            switch (currentState)
            {
                case ActionProcessorState.INITIAL:
                    yield return InitialState(info);
                    yield break;
                case ActionProcessorState.PROCESSING:
                    yield return ProcessingState(info);
                    yield break;
                case ActionProcessorState.CLEANUP:
                    yield return CleanUpState(info);
                    yield break;
                case ActionProcessorState.FINISH:
                    yield return FinishState(info);
                    yield break;
            }
            yield break;
        }

        private IEnumerator InitialState(CombatProcessorInfo info)
        {
            isFinished = true;
            AbilityRequirementsProcessor requirementsProcessor = sourceAbility.Get<AbilityRequirementsProcessor>();
            triggerTool = source.Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.ActionStart);
            foreach (I_AbilityProcessor processor in sourceAbility.GetProcessors())
            {
                processor.OnExecute(source, arguments);
            }
            if (sourceAbility.sourceAbility.name != null)
            {
                CombatLog.Instance.AddMessage(source.gameObject.name + " used " + sourceAbility.sourceAbility.name + "!");
                yield return new WaitForSeconds(.25f);
            }
            currentState = ActionProcessorState.PROCESSING;
            yield break;
        }

        private IEnumerator ProcessingState(CombatProcessorInfo info)
        {
            if (!HasNext(info))
            {
                currentState = ActionProcessorState.CLEANUP;
                yield break;
            }
            List<TargetResult> results = GetNextResults();
            I_CombatProcessor processor = GetNextExecutable();
            if (processor != null)
            {
                sourceParty.GetCurrentBattleContainer().AddInturruptProcessor(CombatProcessorTypes.Instance.COMBAT_ACTION, processor);
                yield break;
            }
            yield break;
        }

        private IEnumerator CleanUpState(CombatProcessorInfo info)
        {
            ListActionBundle listActionBundle = new();
            listActionBundle.Bundles.Add(new BlockingProcessor());
            sourceParty.GetCurrentBattleContainer().AddInturruptProcessor(CombatProcessorTypes.Instance.COMBAT_ACTION, listActionBundle);
            AGenericPool<DeliveryArgumentPacks>.Release(arguments);
            currentState = ActionProcessorState.FINISH;
            yield break;
        }

        private IEnumerator FinishState(CombatProcessorInfo info)
        {
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.ActionEnd);
            currentState = ActionProcessorState.DONE;
            yield break;

        }

        public bool HasNext(CombatProcessorInfo info)
        {
            return targetHolder.HasNextTarget();
        }

        public override bool IsValid(CombatProcessorInfo info)
        {
            if (currentState != ActionProcessorState.DONE)
            {
                FacultyTool fTool = source.Get<FacultyTool>();
                if (!fTool.Can(Faculties.Instance.CHOOSE_ACTION))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public override bool IsFinished(CombatProcessorInfo info)
        {
            FacultyTool fTool = source.Get<FacultyTool>();
            if (!fTool.Can(Faculties.Instance.CHOOSE_ACTION))
            {
                return true;
            }
            return base.IsFinished(info);
        }
    }

    enum ActionProcessorState
    {
        INITIAL, PROCESSING, CLEANUP, FINISH, DONE
    }
}
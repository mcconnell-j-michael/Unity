using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.PartySystem;
using Ashen.PauseSystem;
using Ashen.ToolSystem;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.StateMachineSystem
{
    public class ChooseTargetCombat : I_GameState
    {
        private I_GameState previousState;
        private Ability ability;

        public ChooseTargetCombat(I_GameState sourceState, Ability ability)
        {
            previousState = sourceState;
            this.ability = ability;
        }

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            PlayerInputState inputState = PlayerInputState.Instance;

            ToolManager current = inputState.currentlySelected;
            CombatPortraitManager cpm = CombatPortraitManager.Instance;

            cpm.HideSprite(false);

            AbilityAction abilityAction = ability.abilityAction;
            TargetingProcessor targetingProcessor = abilityAction.Get<TargetingProcessor>();

            Target target = targetingProcessor.GetTargetType(current);
            A_PartyManager targetParty = targetingProcessor.GetTargetParty() == TargetParty.ALLY ? inputState.sourceParty : EnemyPartyHolder.Instance.enemyPartyManager;

            I_TargetHolder targetHolder = target.BuildTargetHolder(current, inputState.sourceParty, targetParty, abilityAction);
            ActionProcessor actionHolder = new(abilityAction, current, inputState.sourceParty, targetParty, targetHolder);

            I_Targetable targetable = targetHolder.GetFirstAvailableTargetable();

            if (targetable == null)
            {
                response.nextState = previousState;
                cpm.DisplaySprite(false);
                yield break;
            }

            ChooseTargetState chooseTarget = new(targetHolder, targetable);

            yield return chooseTarget.RunState(request, response);

            targetable = chooseTarget.GetTargetable();

            if (targetable == null)
            {
                response.nextState = previousState;
                cpm.DisplaySprite(false);
                yield break;
            }

            List<ActionProcessor> actionProcessors = new()
            {
                actionHolder
            };

            SubAbilityProcessor subAbilityProcessor = abilityAction.Get<SubAbilityProcessor>();

            if (subAbilityProcessor != null)
            {
                foreach (AbilityAction subAction in subAbilityProcessor.GetAbilityActions(current))
                {
                    TargetingProcessor processor = subAction.Get<TargetingProcessor>();
                    Target subTarget = processor.GetTargetType(inputState.currentlySelected);
                    SubAbilityTargetingProcessor subTargetProcessor = processor.GetTargetingValue() as SubAbilityTargetingProcessor;
                    A_PartyManager subTargetParty = processor.GetTargetParty() == TargetParty.ALLY ? inputState.sourceParty : EnemyPartyHolder.Instance.enemyPartyManager;
                    I_TargetHolder subTargetHolder;

                    if (subTargetProcessor == null)
                    {
                        subTargetHolder = subTarget.BuildTargetHolder(inputState.currentlySelected, inputState.sourceParty, subTargetParty, subAction);
                        subTargetHolder.GetRandomTargetable();
                    }
                    else if (subTargetProcessor.relativeTarget == SubAbilityRelativeTarget.Self)
                    {
                        subTargetHolder = subTarget.BuildTargetHolder(inputState.currentlySelected, inputState.sourceParty, inputState.sourceParty, subAction);
                        subTargetHolder.SetTargetable(inputState.currentlySelected);
                    }
                    else if (subTargetProcessor.relativeTarget == SubAbilityRelativeTarget.Target)
                    {
                        subTargetHolder = subTarget.BuildTargetHolder(inputState.currentlySelected, inputState.sourceParty, targetParty, subAction);
                        subTargetHolder.SetTargetable(targetable.GetTarget());
                    }
                    else if (subTargetProcessor.relativeTarget == SubAbilityRelativeTarget.Random)
                    {
                        subTargetHolder = subTarget.BuildTargetHolder(inputState.currentlySelected, inputState.sourceParty, subTargetParty, subAction);
                        subTargetHolder.GetRandomTargetable();
                    }
                    else
                    {
                        subTargetHolder = subTarget.BuildTargetHolder(inputState.currentlySelected, inputState.sourceParty, subTargetParty, subAction);
                        subTargetHolder.GetRandomTargetable();
                    }
                    ActionProcessor subActionProcessor = new(subAction, inputState.currentlySelected, inputState.sourceParty, subTargetParty, subTargetHolder);
                    SpeedProcessor speedProcessor = subAction.Get<SpeedProcessor>();
                    subActionProcessor.speedCategory = speedProcessor.GetSpeedCategory();
                    SubAbilitySpeedValue subSpeedProcessor = speedProcessor.GetBaseSpeedProcessor() as SubAbilitySpeedValue;
                    if (subSpeedProcessor != null)
                    {
                        if (subSpeedProcessor.GetRelativeSpeed() == RelativeSpeed.After)
                        {
                            subActionProcessor.speed = actionHolder.speed - .01f;
                        }
                        else if (subSpeedProcessor.GetRelativeSpeed() == RelativeSpeed.Before)
                        {
                            subActionProcessor.speed = actionHolder.speed + .01f;
                        }
                    }
                    actionProcessors.Add(subActionProcessor);
                }
            }

            AbilityRequirementsProcessor requirementsProcessor = abilityAction.Get<AbilityRequirementsProcessor>();

            response.nextState = new AddPlayerAction(actionProcessors, abilityAction, ability.name);
            yield break;
        }
    }
}
using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.DeliverySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    public class ExecuteInputState : SingletonScriptableObject<ExecuteInputState>, I_GameState
    {
        public BattleContainer battleContainer;

        public ExtendedEffectTrigger actionStart;
        public ExtendedEffectTrigger actionEnd;

        public SubactionProcessor currentSubactionProcessor;

        public MonoBehaviour runner;

        public void Reset()
        {
            battleContainer.ClearProcessors();
        }

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            CombatLog.Instance.ClearMessages();
            CombatProcessorInfo info = new(battleContainer, request.runner);
            foreach (AbilitySpeedCategory category in AbilitySpeedProcessOrder.Instance.speedCategoryOrder)
            {
                battleContainer.PopulateByCategory(category);
                yield return new ExecuteProcessors(battleContainer, CombatProcessorTypes.Instance.PRIMARY_ACTION).RunState(request, response);
                if (response.nextState != null)
                {
                    break;
                }
            }
            yield return null;
            if (response.nextState != null)
            {
                yield return WaitForOngoingActions();
            }
            this.Reset();
            if (response.nextState == null)
            {
                response.nextState = EnergyCleanUpState.Instance;
            }
        }

        public IEnumerator WaitForOngoingActions()
        {
            CombatProcessorInfo info = new CombatProcessorInfo(battleContainer, runner);
            List<I_CombatProcessor> ongoingActions = battleContainer.GetProcessors(CombatProcessorTypes.Instance.ONGOING_ACTION);
            for (int x = 0; x < ongoingActions.Count; x++)
            {
                yield return new WaitUntil(() => ongoingActions[x].IsFinished(info));
            }
            ongoingActions.Clear();
        }
    }
}
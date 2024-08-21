using Ashen.CombatSystem;
using Ashen.PartySystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class CheckBattleCondition : I_GameState
    {
        private GameStateResponse response;
        private BattleContainer container;
        private CombatProcessorType type;

        public CheckBattleCondition(BattleContainer battleContainer, CombatProcessorType type)
        {
            this.type = type;
            container = battleContainer;
        }

        public void SetNextState(I_GameState state)
        {
            response.parent.nextState = state;
            response.nextState = new EndCombatState();
        }

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            this.response = response;
            foreach (CombatChecker checker in container.GetCombatCheckers(type))
            {
                if (checker.CheckCondition(PlayerPartyHolder.Instance.partyManager, EnemyPartyHolder.Instance.enemyPartyManager))
                {
                    checker.ExecuteResult(container, this);
                }
                if (response.nextState != null)
                {
                    break;
                }
                yield return null;
            }
            //I_CombatCondition victoryCondition = InitiateCombatState.Instance.victoryCondition;
            //I_CombatCondition failureCondition = InitiateCombatState.Instance.failureCondition;
            //if (failureCondition != null)
            //{
            //    if (failureCondition.ConditionMet(PlayerPartyHolder.Instance.partyManager, EnemyPartyHolder.Instance.enemyPartyManager))
            //    {
            //        response.nextState = InitiateCombatState.Instance.failureState;
            //        yield break;
            //    }
            //}
            //if (victoryCondition != null)
            //{
            //    if (victoryCondition.ConditionMet(PlayerPartyHolder.Instance.partyManager, EnemyPartyHolder.Instance.enemyPartyManager))
            //    {
            //        response.nextState = InitiateCombatState.Instance.victoryState;
            //        yield break;
            //    }
            //}
        }
    }
}
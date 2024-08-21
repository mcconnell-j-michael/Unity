using Ashen.CombatSystem;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.StateMachineSystem
{
    public class ExecuteProcessors : I_GameState
    {
        private BattleContainer battleContainer;
        private CombatProcessorType type;

        public ExecuteProcessors(BattleContainer battleContainer, CombatProcessorType type)
        {
            this.battleContainer = battleContainer;
            this.type = type;
        }

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            List<I_CombatProcessor> processors = battleContainer.GetProcessors(type);
            CombatProcessorInfo info = new(battleContainer, request.runner)
            {
                parentProcessorList = processors,
            };
            while (processors.Count > 0)
            {
                if (response.nextState != null)
                {
                    yield break;
                }
                I_CombatProcessor processor = processors[0];
                if (processor.IsValid(info))
                {
                    yield return new CheckBattleCondition(battleContainer, type).RunState(request, response);
                    if (response.nextState != null)
                    {
                        break;
                    }
                    yield return processor.Execute(info);
                }
                else
                {
                    if (!processor.IsFinished(info))
                    {
                        battleContainer.AddProcesor(CombatProcessorTypes.Instance.ONGOING_ACTION, processor);
                    }
                    processors.RemoveAt(0);
                    yield return null;
                }
                if (type.next != null)
                {
                    yield return new ExecuteProcessors(battleContainer, type.next).RunState(request, response);
                }
            }
            yield return new CheckBattleCondition(battleContainer, type).RunState(request, response);
            if (type.next != null)
            {
                yield return new ExecuteProcessors(battleContainer, type.next).RunState(request, response);
            }
        }
    }
}
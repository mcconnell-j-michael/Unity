using Ashen.DeliverySystem;
using Ashen.PartySystem;
using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class EndRoundState : SingletonScriptableObject<EndRoundState>, I_GameState
    {
        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
            A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;

            ToolManager member = playerParty.GetFirst();

            foreach (PartyPosition position in playerParty.GetActivePositions())
            {
                TriggerTool triggerTool = playerParty.GetToolManager(position).Get<TriggerTool>();
                triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnEnd);
            }
            foreach (PartyPosition position in enemyParty.GetActivePositions())
            {
                TriggerTool triggerTool = enemyParty.GetToolManager(position).Get<TriggerTool>();
                triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnEnd);
            }

            yield return new ExecuteProcessors(ExecuteInputState.Instance.battleContainer, CombatProcessorTypes.Instance.PRIMARY_ACTION).RunState(request, response);

            if (response.nextState == null)
            {
                response.nextState = StartRoundState.Instance;
            }

            yield break;
        }
    }
}
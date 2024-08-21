using Ashen.CombatSystem;
using Ashen.DeliverySystem;
using Ashen.PartySystem;
using Ashen.ToolSystem;
using System.Collections;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    public class StartRoundState : SingletonScriptableObject<StartRoundState>, I_GameState
    {
        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
            A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;

            ToolManager member = playerParty.GetFirst();

            foreach (PartyPosition position in playerParty.GetActivePositions())
            {
                TriggerTool triggerTool = playerParty.GetToolManager(position).Get<TriggerTool>();
                triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnStart);
            }
            foreach (PartyPosition position in enemyParty.GetActivePositions())
            {
                TriggerTool triggerTool = enemyParty.GetToolManager(position).Get<TriggerTool>();
                triggerTool.Trigger(ExtendedEffectTriggers.Instance.TurnStart);
            }
            PlayerInputState.Instance.turn += 1;
            BattleLogUIManager.Instance.turnValue.text = PlayerInputState.Instance.turn.ToString();
            CombatProcessorInfo info = new CombatProcessorInfo(playerParty.GetCurrentBattleContainer(), request.runner);
            yield return new ExecuteProcessors(playerParty.GetCurrentBattleContainer(), CombatProcessorTypes.Instance.SUPPORTING_ACTION).RunState(request, response);
            yield return new CheckBattleCondition(playerParty.GetCurrentBattleContainer(), CombatProcessorTypes.Instance.COMBAT_ACTION);
            if (response.nextState != null)
            {
                yield break;
            }
            response.nextState = PlayerInputState.Instance;
            yield return new WaitForSeconds(1f);
            yield break;
        }
    }
}
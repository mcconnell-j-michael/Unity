using Ashen.CombatSystem;
using Ashen.DeliverySystem;
using Ashen.PartySystem;
using Ashen.ToolSystem;
using System.Collections;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    public class InitiateCombatState : SingletonScriptableObject<InitiateCombatState>, I_GameState
    {
        public EnemyPartyComposition enemyPartyComp;

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            ToolManager[] managers = GameObject.FindObjectsOfType<ToolManager>();

            BattleContainer battleContainer = new();

            ExecuteInputState.Instance.battleContainer = battleContainer;
            ExecuteInputState.Instance.runner = request.runner;

            if (enemyPartyComp.combatCheckers != null)
            {
                foreach (CombatChecker checker in enemyPartyComp.combatCheckers)
                {
                    foreach (CombatProcessorType type in CombatProcessorTypes.Instance)
                    {
                        if (checker.enabledProcessorTypes == null || checker.enabledProcessorTypes.Count == 0 || checker.enabledProcessorTypes.Contains(type))
                        {
                            battleContainer.RegisterCombatChecker(type, checker);
                        }
                    }
                }
            }

            ActionOptionsManager.Instance.gameObject.SetActive(false);

            yield return null;

            A_PartyManager partyManager = PlayerPartyHolder.Instance.partyManager;
            partyManager.RegisterBattleContainer(battleContainer);
            partyManager.Refresh();
            PlayerInputState.Instance.turn = 0;
            BattleLogUIManager.Instance.turnValue.text = "1";
            foreach (PartyPosition position in PartyPositions.Instance)
            {
                ToolManager toolManager = partyManager.GetToolManager(position);
                if (toolManager)
                {
                    TriggerTool triggerTool = toolManager.Get<TriggerTool>();
                    triggerTool.Trigger(ExtendedEffectTriggers.Instance.BattleStart);
                }
            }

            EnemyPartyManager enemyPartyManager = EnemyPartyHolder.Instance.enemyPartyManager;
            enemyPartyManager.RegisterBattleContainer(battleContainer);
            foreach (PartyPosition position in PartyPositions.Instance)
            {
                if (enemyPartyComp.partyComposition.TryGetValue(position, out GameObject prefab))
                {
                    GameObject enemy = Instantiate(prefab, enemyPartyManager.partyMemberManagers[(int)position].transform);
                    ToolManager enemyTool = enemy.GetComponent<ToolManager>();
                    TriggerTool triggerTool = enemyTool.Get<TriggerTool>();
                    triggerTool.Trigger(ExtendedEffectTriggers.Instance.BattleStart);
                    enemyPartyManager.SetToolManager(position, enemyTool);
                }
            }
            enemyPartyManager.Refresh();

            yield return new WaitForSeconds(1f);
            ExecuteInputState.Instance.Reset();
            PlayerInputState.Instance.Initialize();
            GameStateManager gameStateManager = CreateInstance<GameStateManager>();
            gameStateManager.initialState = StartRoundState.Instance;
            yield return gameStateManager.RunState(request, response);
        }
    }
}
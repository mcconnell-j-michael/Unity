using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.PartySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.StateMachineSystem
{
    public class EnemyInputState : SingletonScriptableObject<EnemyInputState>, I_GameState
    {
        public AbilitySO enemyAbility;

        [Hide, Title("Allowed If")]
        public List<I_TargetingRule> allowedIf;

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
            A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;
            ExecuteInputState executeInputState = ExecuteInputState.Instance;

            ToolManager current = enemyParty.GetFirst();

            while (current != null)
            {
                FacultyTool facultyTool = current.Get<FacultyTool>();
                if (!facultyTool.Can(Faculties.Instance.CHOOSE_ACTION))
                {
                    current = enemyParty.GetNext(current);
                    continue;
                }
                Ability ability = enemyAbility.builder.Build();
                AbilityAction abilityAction = ability.abilityAction;
                TargetingProcessor targetingProcessor = abilityAction.Get<TargetingProcessor>();
                Target target = targetingProcessor.GetTargetType(current);
                I_TargetHolder targetHolder = target.BuildTargetHolder(current, enemyParty, playerParty, abilityAction);
                ActionProcessor actionHolder = new ActionProcessor(abilityAction, current, enemyParty, playerParty, targetHolder);
                targetHolder.GetTargetableByThreat();
                playerParty.GetCurrentBattleContainer().AddPrimaryAction(actionHolder);
                current = enemyParty.GetNext(current);
            }

            yield return null;
            response.nextState = new StartCombatRoundState();
        }
    }
}
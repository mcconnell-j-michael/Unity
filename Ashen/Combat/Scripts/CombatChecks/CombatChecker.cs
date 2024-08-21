using Ashen.StateMachineSystem;
using System.Collections.Generic;

namespace Ashen.CombatSystem
{
    public class CombatChecker
    {
        public I_CombatCondition condition;
        public I_CombatResult result;

        [AutoPopulate, EnumSODropdown]
        public List<CombatProcessorType> enabledProcessorTypes;

        public bool CheckCondition(A_PartyManager playerParty, A_PartyManager enemyParty)
        {
            if (condition == null)
            {
                return false;
            }
            return condition.ConditionMet(playerParty, enemyParty);
        }

        public void ExecuteResult(BattleContainer container, CheckBattleCondition checkBattleCondition)
        {
            if (result == null)
            {
                return;
            }
            result.HandleResult(container, checkBattleCondition);
        }
    }
}
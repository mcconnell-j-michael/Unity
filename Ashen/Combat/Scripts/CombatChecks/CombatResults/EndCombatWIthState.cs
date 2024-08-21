using Ashen.StateMachineSystem;

namespace Ashen.CombatSystem
{
    public class EndCombatWIthState : I_CombatResult
    {
        public I_GameState state;

        public void HandleResult(BattleContainer battleContainer, CheckBattleCondition checkBattleCondition)
        {
            checkBattleCondition.SetNextState(state);
        }
    }
}
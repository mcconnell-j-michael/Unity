using Ashen.StateMachineSystem;

namespace Ashen.CombatSystem
{
    public interface I_CombatResult
    {
        void HandleResult(BattleContainer battleContainer, CheckBattleCondition checkBattleCondition);
    }
}
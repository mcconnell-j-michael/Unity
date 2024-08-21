namespace Ashen.CombatSystem
{
    public interface I_CombatCondition
    {
        bool ConditionMet(A_PartyManager playerParty, A_PartyManager enemeyParty);
    }
}
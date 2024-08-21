namespace Ashen.StateMachineSystem
{
    public class CombatProcessorTypes : A_EnumList<CombatProcessorType, CombatProcessorTypes>
    {
        public CombatProcessorType PRIMARY_ACTION;
        public CombatProcessorType COMBAT_ACTION;
        public CombatProcessorType SUPPORTING_ACTION;
        public CombatProcessorType ONGOING_ACTION;
    }
}
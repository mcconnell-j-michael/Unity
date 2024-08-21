using Ashen.PartySystem;
using Ashen.StateMachineSystem;

namespace Ashen.CombatSystem
{
    public class SwapRows : I_CombatResult
    {
        public bool enemy;
        public PartyRow firstRow;
        public PartyRow secondRow;

        private bool swapping = false;

        public void HandleResult(BattleContainer battleContainer, CheckBattleCondition checkBattleCondition)
        {
            if (swapping)
            {
                return;
            }
            swapping = true;
            ListActionBundle bundle = new ListActionBundle();
            bundle.Bundles.Add(new BlockingProcessor());
            bundle.Bundles.Add(new SwapRowsProcessor(
                enemy ? EnemyPartyHolder.Instance.enemyPartyManager : PlayerPartyHolder.Instance.partyManager,
                firstRow,
                secondRow,
                this
            ));
            battleContainer.AddProcesor(CombatProcessorTypes.Instance.COMBAT_ACTION, bundle);
        }

        public void Reset()
        {
            swapping = false;
        }
    }
}
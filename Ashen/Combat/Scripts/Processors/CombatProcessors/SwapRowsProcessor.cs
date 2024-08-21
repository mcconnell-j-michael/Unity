using System.Collections;

namespace Ashen.CombatSystem
{
    public class SwapRowsProcessor : A_CombatProcessor
    {
        private A_PartyManager partyManager;
        private PartyRow firstRow;
        private PartyRow secondRow;

        private SwapRows swapRowsResult;

        public SwapRowsProcessor(A_PartyManager partyManager, PartyRow firstRow, PartyRow secondRow, SwapRows swapRowsResult)
        {
            this.partyManager = partyManager;
            this.firstRow = firstRow;
            this.secondRow = secondRow;
            this.swapRowsResult = swapRowsResult;
        }

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            isFinished = true;
            isValid = false;
            partyManager.SwapRows(firstRow, secondRow);
            swapRowsResult.Reset();
            yield return null;
        }
    }
}
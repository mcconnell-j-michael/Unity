using System.Collections;
using System.Collections.Generic;

namespace Ashen.CombatSystem
{
    public class ListActionBundle : A_CombatProcessor
    {
        private List<I_CombatProcessor> bundles;
        public List<I_CombatProcessor> Bundles
        {
            get
            {
                if (bundles == null)
                {
                    bundles = new List<I_CombatProcessor>();
                }
                return bundles;
            }
        }

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            info.parentProcessorList.InsertRange(0, Bundles);
            isValid = false;
            isFinished = true;
            yield break;
        }
    }
}
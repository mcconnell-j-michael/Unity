using Ashen.StateMachineSystem;
using System.Collections;

namespace Ashen.CombatSystem
{
    public class BlockingProcessor : A_CombatProcessor
    {
        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            yield break;
        }

        public override bool IsFinished(CombatProcessorInfo info)
        {
            return !info.battleContainer.HasProcessors(CombatProcessorTypes.Instance.ONGOING_ACTION, info);
        }

        public override bool IsValid(CombatProcessorInfo info)
        {
            return info.battleContainer.HasProcessors(CombatProcessorTypes.Instance.ONGOING_ACTION, info);
        }
    }
}
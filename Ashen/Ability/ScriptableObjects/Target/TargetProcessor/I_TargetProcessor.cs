using Ashen.CombatSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public interface I_TargetProcessor
    {
        public I_CombatProcessor BuildProcessors(List<TargetResult> targets, AbilityAction action);
    }
}
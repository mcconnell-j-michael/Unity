using Ashen.StateMachineSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class CombatProcessorInfo
    {
        public MonoBehaviour runner;
        public BattleContainer battleContainer;
        public List<I_CombatProcessor> parentProcessorList;

        public CombatProcessorInfo(BattleContainer container, MonoBehaviour runner)
        {
            this.battleContainer = container;
            this.runner = runner;
        }
    }
}
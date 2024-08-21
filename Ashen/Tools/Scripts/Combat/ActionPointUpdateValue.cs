using Ashen.StateMachineSystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ActionPointUpdateValue
    {
        public string name;
        //public List<ActionProcessor> actionProcessors;
        public int actionPointCost;
        public List<I_CharacterCommitment> commitments;
        public bool selected;
    }
}
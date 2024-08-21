using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.CombatSystem
{
    public abstract class A_CombatProcessor : I_CombatProcessor
    {
        public ToolManager owner;

        protected bool isFinished = false;
        protected bool isValid = true;

        private int actionCount = -1;

        public virtual bool IsFinished(CombatProcessorInfo info)
        {
            return isFinished;
        }

        public virtual bool IsValid(CombatProcessorInfo info)
        {
            return isValid;
        }

        public ToolManager GetOwner()
        {
            return owner;
        }

        public int GetActionCount()
        {
            return actionCount;
        }

        public void SetActionCount(int actionCount)
        {
            this.actionCount = actionCount;
        }

        public abstract IEnumerator Execute(CombatProcessorInfo info);
    }
}
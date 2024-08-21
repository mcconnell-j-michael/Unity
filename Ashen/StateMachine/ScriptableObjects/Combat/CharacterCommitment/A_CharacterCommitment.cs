using Ashen.ToolSystem;

namespace Ashen.StateMachineSystem
{
    public abstract class A_CharacterCommitment : I_CharacterCommitment
    {
        private bool committed;
        private int actionCount;
        protected int ActionCount
        {
            get
            {
                return actionCount;
            }
        }

        public A_CharacterCommitment(int actionCount)
        {
            committed = false;
            this.actionCount = actionCount;
        }

        public void OnCommit(ToolManager toolManager)
        {
            if (!committed)
            {
                OnCommitInternal(toolManager);
                committed = true;
            }
        }

        public void Rollback(ToolManager toolManager)
        {
            if (committed)
            {
                RollbackInternal(toolManager);
                committed = false;
            }
        }

        public void Finalize(ToolManager toolManager)
        {
            if (committed)
            {
                FinalizeInternal(toolManager);
                committed = false;
            }
        }

        public void UpdateActionCount(int actionCount)
        {
            int oldCount = this.actionCount;
            this.actionCount = actionCount;
            UpdateActionCountInternal(oldCount, ActionCount);
        }

        protected virtual void UpdateActionCountInternal(int oldActionCount, int newActionCount) { }

        protected abstract void OnCommitInternal(ToolManager toolManager);
        protected abstract void RollbackInternal(ToolManager toolManager);
        protected abstract void FinalizeInternal(ToolManager toolManager);
    }
}
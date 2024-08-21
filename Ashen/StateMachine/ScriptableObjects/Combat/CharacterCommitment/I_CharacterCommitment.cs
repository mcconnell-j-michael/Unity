using Ashen.ToolSystem;

namespace Ashen.StateMachineSystem
{
    public interface I_CharacterCommitment
    {
        void OnCommit(ToolManager toolManager);
        void Rollback(ToolManager toolManager);
        void Finalize(ToolManager toolManager);
        void UpdateActionCount(int actionCount);
    }
}
using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.CombatSystem
{
    public interface I_CombatProcessor
    {
        IEnumerator Execute(CombatProcessorInfo info);
        bool IsValid(CombatProcessorInfo info);
        bool IsFinished(CombatProcessorInfo info);
        ToolManager GetOwner();
        int GetActionCount();
        void SetActionCount(int actionCount);
    }
}
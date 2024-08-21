using Ashen.PauseSystem;
using Ashen.ToolSystem;

namespace Ashen.StateMachineSystem
{
    public class ChoosePauseOptionState : A_ChooseOptionState<PauseOptionsManager, PauseOptionUI>
    {
        public ChoosePauseOptionState(ToolManager toolManager) : base(toolManager)
        {
        }
    }
}
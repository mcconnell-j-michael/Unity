using Ashen.StartMenuSystem;
using Ashen.ToolSystem;

namespace Ashen.StateMachineSystem
{
    public class ChooseStartMenuOptionState : A_ChooseOptionState<StartMenuOptionsManager, StartMenuOptionUI>
    {
        public ChooseStartMenuOptionState(ToolManager toolManager) : base(toolManager)
        {
        }
    }
}
using Ashen.ToolSystem;
using Ashen.UISystem;
using Sirenix.OdinInspector;

namespace Ashen.StartMenuSystem
{
    public class StartOptionExecutor : SerializedMonoBehaviour, I_OptionExecutor
    {
        public I_GameState startGameState;

        public void Deselected(ToolManager source)
        {
        }

        public I_GameState GetGameState(I_GameState parentState)
        {
            return startGameState;
        }

        public void InitializeOption(ToolManager source)
        {
        }

        public void Selected(ToolManager source)
        {
        }
    }
}
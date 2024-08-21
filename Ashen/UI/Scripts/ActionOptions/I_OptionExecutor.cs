using Ashen.ToolSystem;

namespace Ashen.UISystem
{
    public interface I_OptionExecutor
    {
        void InitializeOption(ToolManager source);
        I_GameState GetGameState(I_GameState parentState);
        void Selected(ToolManager source);
        void Deselected(ToolManager source);
    }
}
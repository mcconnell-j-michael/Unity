using Ashen.ToolSystem;

namespace Ashen.AbilitySystem
{
    public interface I_Targetable
    {
        void Selected();
        void SelectedSecondary();
        void Deselected();
        ToolManager GetTarget();
    }
}
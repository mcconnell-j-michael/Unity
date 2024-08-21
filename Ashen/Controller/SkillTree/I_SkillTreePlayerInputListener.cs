namespace Ashen.ControllerSystem
{
    public interface I_SkillTreePlayerInputListener
    {
        void OnSubmit();
        void OnCancel();
        void OnSelectUp();
        void OnSelectDown();
        void OnSelectLeft();
        void OnSelectRight();
        void OnSwitchCharacterLeft();
        void OnSwitchCharacterRight();
    }
}
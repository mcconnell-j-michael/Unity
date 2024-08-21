namespace Ashen.ControllerSystem
{
    public interface I_MenuPlayerInputListener
    {
        void OnSubmit();
        void OnCancel();
        void OnInformation();
        void OnSelectUp();
        void OnSelectDown();
        void OnSelectLeft();
        void OnSelectRight();
        void OnNextButton();
        void OnPreviousButton();
    }
}
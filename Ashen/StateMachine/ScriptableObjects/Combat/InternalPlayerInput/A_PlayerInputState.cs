using Ashen.ControllerSystem;

namespace Ashen.StateMachineSystem
{
    public abstract class A_PlayerInputState : A_InputState, I_MenuPlayerInputListener
    {
        protected override void RegisterInputManager()
        {
            MenuPlayerInputManager inputManager = MenuPlayerInputManager.Instance;
            inputManager.Enable();
            inputManager.RegisterListner(this);
        }

        protected override void UnRegisterInputManager()
        {
            MenuPlayerInputManager inputManager = MenuPlayerInputManager.Instance;
            inputManager.UnRegisterListener(this);
        }

        public virtual void OnCancel() { }

        public virtual void OnInformation() { }

        public virtual void OnSelectDown() { }

        public virtual void OnSelectLeft() { }

        public virtual void OnSelectRight() { }

        public virtual void OnSelectUp() { }

        public virtual void OnSubmit() { }

        public virtual void OnNextButton() { }

        public virtual void OnPreviousButton() { }
    }
}
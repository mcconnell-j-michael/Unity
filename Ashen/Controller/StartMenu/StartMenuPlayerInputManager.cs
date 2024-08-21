using UnityEngine.InputSystem;

namespace Ashen.ControllerSystem
{
    public class StartMenuPlayerInputManager : A_PlayerInputManager<StartMenuPlayerInputManager, I_StartMenuPlayerInputListener>
    {
        private void LateUpdate()
        {
            Cancel = false;
            Submit = false;
        }

        public bool Cancel { get; private set; }
        void OnCancel(InputValue value)
        {
            Cancel = value.isPressed;
        }

        public bool SelectDown { get; private set; }
        void OnSelectDown(InputValue value)
        {
            SelectDown = value.isPressed;
        }

        public bool SelectLeft { get; private set; }
        void OnSelectLeft(InputValue value)
        {
            SelectLeft = value.isPressed;
        }

        public bool SelectRight { get; private set; }
        void OnSelectRight(InputValue value)
        {
            SelectRight = value.isPressed;
        }

        public bool SelectUp { get; private set; }
        void OnSelectUp(InputValue value)
        {
            SelectUp = value.isPressed;
        }

        public bool Submit { get; private set; }
        void OnSubmit(InputValue value)
        {
            Submit = value.isPressed;
        }
    }
}
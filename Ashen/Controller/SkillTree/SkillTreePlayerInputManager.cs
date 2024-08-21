using UnityEngine.InputSystem;

namespace Ashen.ControllerSystem
{
    public class SkillTreePlayerInputManager : A_PlayerInputManager<SkillTreePlayerInputManager, I_SkillTreePlayerInputListener>
    {
        private void Update()
        {
            if (SelectUp)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnSelectUp(); }
            }
            else if (SelectDown)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnSelectDown(); }
            }
            else if (SelectLeft)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnSelectLeft(); }
            }
            else if (SelectRight)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnSelectRight(); }
            }
        }

        private void LateUpdate()
        {
            Cancel = false;
            Submit = false;
            SwitchCharacterLeft = false;
            SwitchCharacterRight = false;
        }

        public bool Cancel { get; private set; }
        void OnCancel(InputValue value)
        {
            if (value.isPressed)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnCancel(); }
            }
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
            if (value.isPressed)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnSubmit(); }
            }
            Submit = value.isPressed;
        }

        public bool SwitchCharacterLeft { get; private set; }
        void OnSwitchCharacterLeft(InputValue value)
        {
            if (value.isPressed)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnSwitchCharacterLeft(); }
            }
            SwitchCharacterLeft = value.isPressed;
        }

        public bool SwitchCharacterRight { get; private set; }
        void OnSwitchCharacterRight(InputValue value)
        {
            if (value.isPressed)
            {
                foreach (I_SkillTreePlayerInputListener listener in listeners) { listener.OnSwitchCharacterRight(); }
            }
            SwitchCharacterRight = value.isPressed;
        }
    }
}
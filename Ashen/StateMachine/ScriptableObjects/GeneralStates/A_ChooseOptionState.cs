using Ashen.ControllerSystem;
using Ashen.ToolSystem;
using Ashen.UISystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class A_ChooseOptionState<T, E> : A_InputState, I_MenuPlayerInputListener where T : A_OptionsManager<T, E> where E : A_OptionUI
    {
        protected E previousSubmittedOption;
        private int currentOptionIndex;
        private E[] options;

        protected ToolManager toolManager;

        public A_ChooseOptionState(ToolManager toolManager)
        {
            this.toolManager = toolManager;
        }

        protected override IEnumerator PreProcessState()
        {
            T optionsManager = A_OptionsManager<T, E>.Instance;
            optionsManager.Restart();
            optionsManager.RegisterToolManager(toolManager);
            options = optionsManager.GetOptionsInOrder();
            currentOptionIndex = 0;
            if (previousSubmittedOption != null)
            {
                E firstOption = previousSubmittedOption;
                for (int x = 0; x < options.Length; x++)
                {
                    if (firstOption == options[x])
                    {
                        currentOptionIndex = x;
                    }
                }
            }

            foreach (E option in options)
            {
                if (options[currentOptionIndex] != option)
                {
                    option.Deselected();
                    option.GradientEnabled(false);
                    option.optionExecutor?.Deselected(toolManager);
                }
            }

            options[currentOptionIndex].Selected();
            options[currentOptionIndex].GradientEnabled(true);
            options[currentOptionIndex].optionExecutor?.Selected(toolManager);

            yield break;
        }

        public E GetPreviousOption()
        {
            return previousSubmittedOption;
        }

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

        public void OnSubmit()
        {
            E option = options[currentOptionIndex];
            if (!option.Valid)
            {
                return;
            }
            response.nextState = option.GetGameState(this);
            option.OnSubmit();
            if (response.nextState != null)
            {
                previousSubmittedOption = options[currentOptionIndex];
            }
        }

        public void OnCancel()
        {
            if (CanCancel())
            {
                response.nextState = CancelledState();
            }
        }

        public virtual void OnInformation() { }

        public void OnSelectDown()
        {
            if (IsDelayed())
            {
                return;
            }
            int nextIndex = currentOptionIndex + 1;
            if (nextIndex == options.Length)
            {
                nextIndex = 0;
            }
            SwapOptions(nextIndex);
        }

        public void OnSelectLeft()
        {
        }

        public void OnSelectRight()
        {
        }

        public void OnSelectUp()
        {
            if (IsDelayed())
            {
                return;
            }
            int nextIndex = currentOptionIndex - 1;
            if (nextIndex < 0)
            {
                nextIndex = options.Length - 1;
            }
            SwapOptions(nextIndex);
        }

        private void SwapOptions(int nextIndex)
        {
            E currentOption = options[currentOptionIndex];
            E nextOption = options[nextIndex];
            if (nextOption != null && currentOption != nextOption)
            {
                currentOptionIndex = nextIndex;
                currentOption.Deselected();
                currentOption.GradientEnabled(false);
                currentOption.optionExecutor?.Deselected(toolManager);
                nextOption.Selected();
                nextOption.GradientEnabled(true);
                nextOption.optionExecutor?.Selected(toolManager);
                SetSelectDelay(0.15f);
            }
        }

        protected virtual bool CanCancel()
        {
            return true;
        }

        protected virtual I_GameState CancelledState()
        {
            return new ExitState();
        }

        public virtual void OnNextButton() { }

        public virtual void OnPreviousButton() { }
    }
}
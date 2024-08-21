using JoshH.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.UISystem
{
    public abstract class A_OptionUI : SerializedMonoBehaviour
    {
        public I_OptionExecutor optionExecutor;
        public Image background;
        public Image border;
        public TextMeshProUGUI title;
        [SerializeField]
        private UIGradient gradient;

        protected bool valid;
        private bool selected;
        private bool wasValid;

        public bool Valid
        {
            get
            {
                return valid;
            }
            set
            {
                I_OptionsManager manager = GetManager();
                if (value)
                {
                    border.color = manager.GetValidOptions().border;
                    background.color = manager.GetValidOptions().background;
                    title.color = manager.GetValidOptions().title;
                    gradient.LinearColor1 = manager.GetValidOptions().color1;
                    gradient.LinearColor2 = manager.GetValidOptions().color2;
                }
                else
                {
                    border.color = manager.GetInvalidOptions().border;
                    background.color = manager.GetInvalidOptions().background;
                    title.color = manager.GetInvalidOptions().title;
                    gradient.LinearColor1 = manager.GetInvalidOptions().color1;
                    gradient.LinearColor2 = manager.GetInvalidOptions().color2;
                }
                valid = value;
            }
        }

        private void Start()
        {
            gradient.enabled = false;
            selected = false;
            Initialize();
        }

        public void Restart()
        {
            gradient.enabled = false;
        }

        public void GradientEnabled(bool enabled)
        {
            gradient.enabled = enabled;
        }

        public void Selected()
        {
            SelectedInternal();
            if ((!selected || !wasValid) && valid)
            {
                Reselect();
                wasValid = true;
            }
            if (!selected)
            {
                Select();
                selected = true;
            }
        }

        public void Deselected()
        {
            DeselectedInternal();
            if (selected)
            {
                if (valid || wasValid)
                {
                    Invalidate();
                }
                Deslect();
                wasValid = false;
                selected = false;
            }
        }

        public I_GameState GetGameState(I_GameState parentState)
        {
            if (optionExecutor == null)
            {
                return null;
            }
            return optionExecutor.GetGameState(parentState);
        }

        protected bool IsSelected()
        {
            return selected;
        }

        public virtual void OnSubmit() { }
        public virtual void OnReselected() { }

        protected abstract I_OptionsManager GetManager();

        protected virtual void Initialize() { }
        protected virtual void SelectedInternal() { }
        protected virtual void Reselect() { }
        protected virtual void Select() { }
        protected virtual void DeselectedInternal() { }
        protected virtual void Invalidate() { }
        protected virtual void Deslect() { }
    }
}
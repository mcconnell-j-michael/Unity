using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;

namespace Ashen.UISystem
{
    public abstract class A_OptionsManager<T, E> : SingletonMonoBehaviour<T>, I_OptionsManager where T : A_OptionsManager<T, E> where E : A_OptionUI
    {
        [NonSerialized]
        public E previouslySelected;
        [NonSerialized]
        public E currentlySelected;

        [Hide, FoldoutGroup("Color Scheme"), Title("Valid")]
        public ActionOptionColorScheme validOptionColors;
        [Hide, FoldoutGroup("Color Scheme"), Title("Invalid")]
        public ActionOptionColorScheme invalidOptionColors;

        protected E[] optionsInOrder;

        public void Start()
        {
            optionsInOrder = GetComponentsInChildren<E>();
            Initialize();
        }

        public void RegisterToolManager(ToolManager toolManager)
        {
            foreach (A_OptionUI optionUI in optionsInOrder)
            {
                if (optionUI.optionExecutor != null)
                {
                    optionUI.optionExecutor.InitializeOption(toolManager);
                }
            }
        }

        public void Initialize()
        {
            foreach (A_OptionUI optionUI in optionsInOrder)
            {
                if (optionUI.optionExecutor != null)
                {
                    optionUI.Valid = true;
                }
                else
                {
                    optionUI.Valid = false;
                }
            }
        }

        public E[] GetOptionsInOrder()
        {
            return optionsInOrder;
        }

        public void Restart()
        {
            foreach (E option in optionsInOrder)
            {
                option.Restart();
            }
        }

        public ActionOptionColorScheme GetValidOptions()
        {
            return validOptionColors;
        }

        public ActionOptionColorScheme GetInvalidOptions()
        {
            return invalidOptionColors;
        }
    }
}
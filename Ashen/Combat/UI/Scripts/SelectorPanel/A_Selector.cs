using Ashen.AbilitySystem;
using JoshH.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public abstract class A_Selector : MonoBehaviour
    {
        public UIGradient gradient;
        public Ability ability;
        public TextMeshProUGUI abilityName;
        public Image background;


        private bool valid;
        public bool Valid
        {
            get
            {
                return valid;
            }
            set
            {
                if (value)
                {
                    OnValid();
                }
                else
                {
                    OnInValid();
                }
                valid = value;
                OnSetValid(value);
            }
        }

        protected virtual void OnValid() { }
        protected virtual void OnInValid() { }

        public void OnCreate()
        {
            RectTransform skillRect = transform as RectTransform;
            abilityName.rectTransform.sizeDelta = new Vector2(skillRect.rect.width / 2f, abilityName.rectTransform.sizeDelta.y);
        }

        protected virtual void InternalOnCreate()
        {

        }

        public void Deselected()
        { }

        public void GradientEnabled(bool enabled)
        {
            gradient.enabled = enabled;
        }

        public virtual void OnSetValid(bool valid) { }
    }
}
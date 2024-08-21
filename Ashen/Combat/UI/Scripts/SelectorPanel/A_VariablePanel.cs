using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public abstract class A_VariablePanel<T, E> : A_SelectorPanel<T, E> where T : A_VariablePanel<T, E> where E : A_Selector
    {
        public GameObject selectorPrefab;

        public ScrollRect scrollRect;
        public RectTransform selectorPanelRect;
        public RectTransform fullContentRect;

        private int windowStart;

        public float initialOffset;
        public float offsetPerSelector;

        public int numberOfSelectors;

        protected float height;

        protected override void Initialize()
        {
            RectTransform abilityRect = selectorPrefab.GetComponent<RectTransform>();
            height = abilityRect.rect.height;
        }

        public override void UpdateSelection(E selector)
        {
            int index = selectors.IndexOf(selector);
            if (index >= windowStart && index < windowStart + numberOfSelectors)
            {
                return;
            }
            if (index == 0)
            {
                windowStart = 0;
            }
            else if (index == selectors.Count - 1)
            {
                windowStart = index - numberOfSelectors + 1;
            }
            else if (index < windowStart)
            {
                windowStart--;
            }
            else
            {
                windowStart++;
            }
            int total = windowStart;
            float scrollHeight = (height * total) + (offsetPerSelector * total);
            float percentage = 1 - (scrollHeight / (fullContentRect.sizeDelta.y - (numberOfSelectors * height) - (numberOfSelectors * offsetPerSelector) - initialOffset));
            scrollRect.verticalNormalizedPosition = percentage;
        }
    }
}

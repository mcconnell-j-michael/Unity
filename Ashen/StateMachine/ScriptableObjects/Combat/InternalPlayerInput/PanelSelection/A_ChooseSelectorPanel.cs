using Ashen.CombatSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    public abstract class A_ChooseSelectorPanel<T, E> : A_PlayerInputState where T : A_SelectorPanel<T, E> where E : A_Selector
    {
        private I_GameState previousState;
        protected T panelHandler;

        protected int currentSelectorIdx;
        protected List<E> selectors;

        public A_ChooseSelectorPanel(I_GameState previousState)
        {
            this.previousState = previousState;
        }

        public abstract IEnumerator GetPanelHandler();

        public virtual void Reset() { }
        public virtual void OnSubmitInternal() { }

        protected override IEnumerator PreProcessState()
        {
            panelHandler = null;

            yield return GetPanelHandler();

            if (panelHandler == null)
            {
                response.nextState = previousState;
                yield break; ;
            }

            ActionOptionsManager.Instance.HideCurrent();
            yield return new WaitForSeconds(.25f);

            panelHandler.enabler.SetActive(true);

            currentSelectorIdx = 0;
            selectors = panelHandler.selectors;

            foreach (E selector in selectors)
            {
                selector.GradientEnabled(false);
            }

            PreProcessStateInternal();

            panelHandler.UpdateSelection(selectors[currentSelectorIdx]);
            selectors[currentSelectorIdx].GradientEnabled(true);
        }

        protected virtual IEnumerator PreProcessStateInternal()
        {
            yield break;
        }

        protected override void PostProcessState()
        {
            ActionOptionsManager.Instance.ShowCurrent();
        }

        public override void OnSubmit()
        {
            E currentSelector = selectors[currentSelectorIdx];
            currentSelector.GradientEnabled(false);
            panelHandler.enabler.SetActive(false);
            OnSubmitInternal();
        }

        public override void OnCancel()
        {
            selectors[currentSelectorIdx].GradientEnabled(false);
            response.nextState = previousState;
            panelHandler.enabler.SetActive(false);
        }

        public override void OnSelectUp()
        {
            SelectDirection(currentSelectorIdx - 1);
        }

        public override void OnSelectDown()
        {
            SelectDirection(currentSelectorIdx + 1);
        }

        private void SelectDirection(int newIndex)
        {
            if (IsDelayed())
            {
                return;
            }
            SetSelectDelay(0.15f);
            E current = selectors[currentSelectorIdx];
            if (newIndex >= panelHandler.activeAbilities)
            {
                newIndex = 0;
            }
            else if (newIndex < 0)
            {
                newIndex = panelHandler.activeAbilities - 1;
            }
            E next = selectors[newIndex];
            currentSelectorIdx = newIndex;
            current.GradientEnabled(false);
            panelHandler.UpdateSelection(next);
            next.GradientEnabled(true);
        }
    }
}

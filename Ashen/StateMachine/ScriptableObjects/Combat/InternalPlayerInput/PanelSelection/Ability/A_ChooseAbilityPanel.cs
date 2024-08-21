using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public abstract class A_ChooseAbilityPanel<T, E> : A_ChooseSelectorPanel<T, E> where T : A_AbilityPanelHandler<T, E> where E : A_AbilitySelector
    {
        protected Ability previousAbility;

        public A_ChooseAbilityPanel(I_GameState previousState) : base(previousState) { }

        protected override IEnumerator PreProcessStateInternal()
        {
            if (previousAbility != null)
            {
                E selector = panelHandler.GetSelectorForAbility(previousAbility);
                currentSelectorIdx = selectors.IndexOf(selector);
                Reset();
            }
            yield break;
        }

        protected override void PostProcessState()
        {
            ActionOptionsManager.Instance.ShowCurrent();
        }

        public override void OnSubmitInternal()
        {
            E currentSelector = selectors[currentSelectorIdx];
            response.nextState = new ChooseTargetCombat(this, currentSelector.ability);
            previousAbility = currentSelector.ability;
        }
    }
}
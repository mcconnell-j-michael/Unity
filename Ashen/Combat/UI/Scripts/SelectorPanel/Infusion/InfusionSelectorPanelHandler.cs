using System.Collections.Generic;

namespace Ashen.CombatSystem
{
    public class InfusionSelectorPanelHandler : A_SelectorPanel<InfusionSelectorPanelHandler, InfusionPanelSelector>
    {
        protected override void RegisterSelector(InfusionPanelSelector selector)
        { }

        protected override void Initialize()
        {
            InfusionPanelSelector[] foundSelectors = selectorHolder.GetComponentsInChildren<InfusionPanelSelector>(true);
            selectors = new List<InfusionPanelSelector>();
            selectors.AddRange(foundSelectors);
        }
    }
}
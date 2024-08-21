using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class SkillChooseAbilityPanel : A_ChooseAbilityPanel<SkillAbilityPanelHandler, SkillAbilitySelector>
    {
        public SkillChooseAbilityPanel(I_GameState previousState) : base(previousState)
        { }

        public override IEnumerator GetPanelHandler()
        {
            PlayerInputState inputState = PlayerInputState.Instance;
            AbilityHolder abilityHolder = inputState.currentlySelected.Get<AbilityTool>().AbilityHolder;

            if (abilityHolder.GetCount() == 0)
            {
                yield break;
            }

            SkillAbilityPanelHandler skillPanel = SkillAbilityPanelHandler.Instance;
            skillPanel.RegisterToolManager(inputState.currentlySelected);
            yield return skillPanel.LoadAbilities(abilityHolder);
            panelHandler = skillPanel;
        }
    }
}
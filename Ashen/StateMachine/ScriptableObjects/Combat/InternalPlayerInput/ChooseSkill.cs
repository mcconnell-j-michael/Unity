using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSkill : A_PlayerInputState
{
    private I_GameState previousState;
    private Ability previousAbility;

    private I_GameState lastTurnNextState;

    private int currentlySelectedSkill;
    private SkillAbilityPanelHandler skillPanel;
    private List<SkillAbilitySelector> skillSelectors;

    protected override IEnumerator PreProcessState()
    {
        PlayerInputState inputState = PlayerInputState.Instance;
        AbilityHolder abilityHolder = inputState.currentlySelected.Get<AbilityTool>().AbilityHolder;
        if (abilityHolder.GetCount() == 0)
        {
            response.nextState = previousState;
            yield break;
        }
        ActionOptionsManager.Instance.HideCurrent();
        yield return new WaitForSeconds(.25f);

        skillPanel = SkillAbilityPanelHandler.Instance;
        skillPanel.RegisterToolManager(inputState.currentlySelected);
        skillPanel.enabler.SetActive(true);
        yield return skillPanel.LoadAbilities(abilityHolder);

        currentlySelectedSkill = 0;
        skillSelectors = skillPanel.selectors;

        foreach (SkillAbilitySelector skill in skillSelectors)
        {
            skill.GradientEnabled(false);
        }

        if (previousAbility != null)
        {
            currentlySelectedSkill = skillSelectors.IndexOf(skillPanel.GetSelectorForAbility(previousAbility));
        }

        skillPanel.UpdateSelection(skillSelectors[currentlySelectedSkill]);
        skillSelectors[currentlySelectedSkill].GradientEnabled(true);
    }

    protected override void PostProcessState()
    {
        ActionOptionsManager.Instance.ShowCurrent();
    }

    public ChooseSkill(I_GameState previousState)
    {
        this.previousState = previousState;
    }

    public override void OnSubmit()
    {
        response.nextState = new ChooseTargetCombat(this, skillSelectors[currentlySelectedSkill].ability);
        lastTurnNextState = response.nextState;
        previousAbility = skillSelectors[currentlySelectedSkill].ability;
        skillSelectors[currentlySelectedSkill].GradientEnabled(false);
        skillPanel.enabler.SetActive(false);
    }

    public override void OnCancel()
    {
        CombatTool ct = PlayerInputState.Instance.currentlySelected.Get<CombatTool>();
        ct.RemoveLastCommitment();
        skillSelectors[currentlySelectedSkill].GradientEnabled(false);
        response.nextState = previousState;
        skillPanel.enabler.SetActive(false);
    }

    public override void OnSelectUp()
    {
        SelectDirection(currentlySelectedSkill - 1);
    }

    public override void OnSelectDown()
    {
        SelectDirection(currentlySelectedSkill + 1);
    }

    private void SelectDirection(int newIndex)
    {
        if (IsDelayed())
        {
            return;
        }
        SetSelectDelay(0.15f);
        SkillAbilitySelector currentSkill = skillSelectors[currentlySelectedSkill];
        if (newIndex >= skillPanel.activeAbilities)
        {
            newIndex = 0;
        }
        else if (newIndex < 0)
        {
            newIndex = skillPanel.activeAbilities - 1;
        }
        SkillAbilitySelector nextSkill = skillSelectors[newIndex];
        currentlySelectedSkill = newIndex;
        currentSkill.GradientEnabled(false);
        skillPanel.UpdateSelection(nextSkill);
        nextSkill.GradientEnabled(true);
    }
}

using Ashen.PauseSystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using System.Collections;
using UnityEngine;

public class ChooseCombatOption : A_ChooseOptionState<ActionOptionsManager, CombatOptionUI>
{
    public ChooseCombatOption(ToolManager toolManager, CombatOptionUI previousOption) : base(toolManager)
    {
        previousSubmittedOption = previousOption;
    }

    protected override IEnumerator PreProcessState()
    {
        CombatPortraitManager cpm = CombatPortraitManager.Instance;
        float delay = 0f;
        if (!cpm.IsRegistered(toolManager))
        {
            delay = cpm.HideSprite(true);
        }
        SetSelectDelay(Mathf.Max(0.1f, delay), DelayGroup.GROUP_TWO);
        yield return base.PreProcessState();
    }

    protected override void OnSelectDelayFinished(DelayGroup delayGroup)
    {
        if (delayGroup == DelayGroup.GROUP_TWO)
        {
            CombatPortraitManager cpm = CombatPortraitManager.Instance;
            cpm.RegisterToolManager(toolManager);
            cpm.DisplaySprite(false);
        }
    }

    public override void OnInformation()
    {
        response.nextState = new NavigateActionPoints(this);
    }

    protected override I_GameState CancelledState()
    {
        return new MovePreviousTurn();
    }

    public override void OnNextButton()
    {
        if (IsDelayed(DelayGroup.GROUP_TWO))
        {
            return;
        }
        PlayerInputState.Instance.RequestMoveNextCharacter();
        response.nextState = new ExitState();
    }

    public override void OnPreviousButton()
    {
        if (IsDelayed(DelayGroup.GROUP_TWO))
        {
            return;
        }
        PlayerInputState.Instance.RequestMovePreviousCharacter();
        response.nextState = new ExitState();
    }
}

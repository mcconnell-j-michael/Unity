using Ashen.AbilitySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using Ashen.UISystem;
using UnityEngine;

public class SkillOptionExecutor : MonoBehaviour, I_OptionExecutor
{
    public CombatOptionUI combatOption;

    public void InitializeOption(ToolManager source)
    {
        AbilityHolder abilityHolder = source.Get<AbilityTool>().AbilityHolder;
        if (abilityHolder != null)
        {
            if (abilityHolder.GetCount() > 0)
            {
                combatOption.Valid = true;
            }
            else
            {
                combatOption.Valid = false;
            }
        }
        else
        {
            combatOption.Valid = false;
        }
    }

    public I_GameState GetGameState(I_GameState parentState)
    {
        return new SkillChooseAbilityPanel(parentState);
    }

    public void Selected(ToolManager source)
    {
    }

    public void Deselected(ToolManager source)
    {
    }
}

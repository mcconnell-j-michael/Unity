using Ashen.AbilitySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using Ashen.UISystem;
using UnityEngine;

namespace Ashen.PauseSystem
{
    public class CustomOptionExecutor : MonoBehaviour, I_OptionExecutor
    {
        public A_OptionUI pauseOption;
        public AbilitySO chooseCharacterAbility;
        public GameObject skillTreeUIGO;
        public GameObject portraitGO;

        public void Deselected(ToolManager source)
        {
        }

        public I_GameState GetGameState(I_GameState parentState)
        {
            return new InitiateSkillTreeState(parentState, chooseCharacterAbility.builder.Build(), skillTreeUIGO, portraitGO);
        }

        public void InitializeOption(ToolManager source)
        {
            pauseOption.Valid = true;
        }

        public void Selected(ToolManager source)
        {
        }
    }
}
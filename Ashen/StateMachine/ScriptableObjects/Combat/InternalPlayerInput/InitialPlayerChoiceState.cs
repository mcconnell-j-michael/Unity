using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.StateMachineSystem
{
    public class InitialPlayerChoiceState : I_GameState
    {
        private CombatOptionUI previousOption;

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            PlayerInputState inputState = PlayerInputState.Instance;
            ToolManager current = inputState.currentlySelected;
            ChooseCombatOption chooseCombatOption = new(current, previousOption);
            yield return chooseCombatOption.RunState(request, response);
            previousOption = chooseCombatOption.GetPreviousOption();
        }
    }
}
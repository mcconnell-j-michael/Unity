using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using Ashen.UISystem;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class MoveOptionExecutor : MonoBehaviour, I_OptionExecutor
    {
        public CombatOptionUI combatOption;

        public void Deselected(ToolManager source)
        {
        }

        public I_GameState GetGameState(I_GameState parentState)
        {
            return new MoveCharacters();
        }

        public void InitializeOption(ToolManager source)
        {
            combatOption.Valid = true;
        }

        public void Selected(ToolManager source)
        {
        }
    }
}
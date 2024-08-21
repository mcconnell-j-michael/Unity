using Ashen.AbilitySystem;
using Ashen.PartySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using Ashen.UISystem;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class ItemOptionExecutor : MonoBehaviour, I_OptionExecutor
    {
        public CombatOptionUI combatOption;

        public void InitializeOption(ToolManager source)
        {
            PlayerPartyManager manager = PlayerPartyHolder.Instance.partyManager;
            AbilityHolder abilityHolder = manager.partyToolManager.Get<PartyItemsManager>().AbilityHolder;
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
            return new ItemChooseAbilityPanel(parentState);
        }

        public void Selected(ToolManager source)
        {
        }

        public void Deselected(ToolManager source)
        {
        }
    }
}
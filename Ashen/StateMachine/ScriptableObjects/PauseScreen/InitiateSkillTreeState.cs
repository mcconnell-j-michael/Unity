using Ashen.AbilitySystem;
using System.Collections;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    public class InitiateSkillTreeState : I_GameState
    {
        private I_GameState previousState;
        private Ability chooseCharacterAbility;
        private GameObject skillTreeUIGO;
        private GameObject portraitGO;

        public InitiateSkillTreeState(I_GameState previousState, Ability chooseCharacterAbility, GameObject skillTreeUIGO, GameObject portraitGO)
        {
            this.previousState = previousState;
            this.chooseCharacterAbility = chooseCharacterAbility;
            this.skillTreeUIGO = skillTreeUIGO;
            this.portraitGO = portraitGO;
        }

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;

            PartyPosition pos = playerParty.GetFirstEnabledPartyPosition();

            AbilityAction abilityAction = chooseCharacterAbility.abilityAction;
            TargetingProcessor targetingProcessor = abilityAction.Get<TargetingProcessor>();

            Target target = targetingProcessor.GetTargetType(null);

            I_TargetHolder targetHolder = target.BuildTargetHolder(null, playerParty, playerParty, abilityAction);
            I_Targetable targetable = targetHolder.GetFirstAvailableTargetable();

            if (targetable == null)
            {
                response.nextState = previousState;
                yield break;
            }

            ChooseTargetState chooseTarget = new(targetHolder, targetable);

            yield return chooseTarget.RunState(request, response);

            targetable = chooseTarget.GetTargetable();

            if (targetable == null)
            {
                response.nextState = previousState;
                yield break;
            }

            skillTreeUIGO.SetActive(true);
            portraitGO.SetActive(true);

            yield return new SkillTreeState(targetable.GetTarget(), targetHolder, targetable).RunState(request, response);

            skillTreeUIGO.SetActive(false);
            portraitGO.SetActive(false);

            yield return null;

            response.nextState = previousState;

        }
    }
}
using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.StateMachineSystem
{
    public class AddPlayerAction : I_GameState
    {
        private List<ActionProcessor> actionProcessors;
        private AbilityAction abilityAction;
        private string name;

        public AddPlayerAction(List<ActionProcessor> actionHolder, AbilityAction abilityAction, string name)
        {
            this.actionProcessors = actionHolder;
            this.abilityAction = abilityAction;
            this.name = name;
        }

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            PlayerInputState inputState = PlayerInputState.Instance;
            ExecuteInputState executeInputState = ExecuteInputState.Instance;

            ActionCommitment actionCommitment = new(actionProcessors, abilityAction, name, inputState.GetActionCount());
            //ActionPointCommitment commitment = new ActionPointCommitment(1, inputState.GetActionCount());
            CombatTool ct = PlayerInputState.Instance.currentlySelected.Get<CombatTool>();
            ct.AddCommitment(actionCommitment);

            //foreach (ActionProcessor processor in actionProcessors)
            //{
            //    ActionCommitment actionCommitment = new ActionCommitment(processor, inputState.GetActionCount());
            //    ct.AddCommitment(actionCommitment);
            //}

            ResourceValueTool rvTool = inputState.currentlySelected.Get<ResourceValueTool>();
            rvTool.ClearTempValues(ResourceValues.Instance.ACTION_POINT, ThresholdValueTempCategories.Instance.PREVIEW);
            response.nextState = new MoveNextTurn();
            yield break;
        }
    }
}
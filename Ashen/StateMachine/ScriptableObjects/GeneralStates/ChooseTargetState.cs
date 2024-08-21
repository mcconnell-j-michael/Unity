using Ashen.AbilitySystem;
using UnityEngine.EventSystems;

namespace Ashen.StateMachineSystem
{
    public class ChooseTargetState : A_PlayerInputState
    {
        private I_TargetHolder targetHolder;
        private I_Targetable currentTarget;

        public ChooseTargetState(I_TargetHolder targetHolder, I_Targetable defaultTarget)
        {
            targetHolder.InitializeTarget(defaultTarget);
            this.targetHolder = targetHolder;
            currentTarget = defaultTarget;
        }

        public override void OnCancel()
        {
            targetHolder.Cleanup();
            currentTarget = null;
            response.nextState = new ExitState();
        }

        public override void OnSelectUp()
        {
            SelectDirection(MoveDirection.Up);
        }

        public override void OnSelectDown()
        {
            SelectDirection(MoveDirection.Down);
        }

        public override void OnSelectLeft()
        {
            SelectDirection(MoveDirection.Left);
        }

        public override void OnSelectRight()
        {
            SelectDirection(MoveDirection.Right);
        }

        public override void OnSubmit()
        {
            targetHolder.Cleanup();
            response.nextState = new ExitState();
        }

        public I_Targetable GetTargetable()
        {
            return currentTarget;
        }

        private void SelectDirection(MoveDirection direction)
        {
            if (IsDelayed())
            {
                return;
            }
            SetSelectDelay(0.15f);
            currentTarget = targetHolder.RequestMove(direction);
        }
    }
}
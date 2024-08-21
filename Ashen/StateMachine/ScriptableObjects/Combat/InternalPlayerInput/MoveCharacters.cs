using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections;
using UnityEngine.EventSystems;

namespace Ashen.StateMachineSystem
{
    public class MoveCharacters : A_PlayerInputState
    {
        private PartyUIManager uiManager;
        private PlayerPartyManager manager;

        private PartyPosition hovered;
        private PartyPosition currentlySelectedPosition;
        private PartyPosition currentlyAt;

        private ToolManager currentlySelectedToolManager;

        protected override IEnumerator PreProcessState()
        {
            PlayerInputState inputState = PlayerInputState.Instance;

            manager = PlayerPartyHolder.Instance.partyManager;
            uiManager = manager.GetPartyUIManager() as PartyUIManager;
            currentlySelectedToolManager = null;
            currentlySelectedPosition = null;

            currentlyAt = null;

            hovered = manager.GetFirstEnabledPartyPosition();
            uiManager.GetTargetable(hovered).Selected();
            uiManager.positionToManager[manager.GetPosition(inputState.currentlySelected)].TurnSelectionEnd();

            uiManager.ShowAll = true;

            yield break;
        }

        protected override void PostProcessState()
        {
            PlayerInputState inputState = PlayerInputState.Instance;
            inputState.RequestRestart();
            uiManager.ShowAll = false;
        }

        private void SelectDirection(MoveDirection direction)
        {
            if (IsDelayed())
            {
                return;
            }
            SetSelectDelay(0.15f);
            PartyPosition focus = hovered != null ? hovered : currentlyAt;
            PartyPosition next = PartyPositions.Instance.GetValidPosition(manager.enabledPositions, focus, direction, false);
            if (next == null)
            {
                return;
            }
            if (hovered != null)
            {
                uiManager.GetTargetable(hovered).Deselected();
                uiManager.GetTargetable(next).Selected();
                hovered = next;
                return;
            }
            if (currentlyAt != currentlySelectedPosition)
            {
                ToolManager otherTM = manager.GetToolManager(currentlySelectedPosition);
                manager.SetToolManager(currentlyAt, otherTM);
                uiManager.SetPartyMember(currentlyAt, otherTM);
            }
            ToolManager newTM = manager.GetToolManager(next);
            manager.SetToolManager(currentlySelectedPosition, newTM);
            uiManager.SetPartyMember(currentlySelectedPosition, newTM);

            manager.SetToolManager(next, currentlySelectedToolManager);
            uiManager.SetPartyMember(next, currentlySelectedToolManager);

            uiManager.GetTargetable(currentlyAt).Deselected();
            uiManager.positionToManager[currentlyAt].TurnSelectionEnd();

            currentlyAt = next;

            uiManager.GetTargetable(currentlyAt).Selected();
            uiManager.positionToManager[currentlyAt].TurnSelectionStart();
        }

        public override void OnCancel()
        {
            if (hovered != null)
            {
                response.nextState = null;
                uiManager.GetTargetable(hovered).Deselected();
                response.nextState = new ExitState();
            }
            else
            {
                ToolManager other = manager.GetToolManager(currentlySelectedPosition);
                manager.SetToolManager(currentlyAt, other);
                uiManager.SetPartyMember(currentlyAt, other);

                manager.SetToolManager(currentlySelectedPosition, currentlySelectedToolManager);
                uiManager.SetPartyMember(currentlySelectedPosition, currentlySelectedToolManager);
                uiManager.GetTargetable(currentlyAt).Deselected();
                uiManager.positionToManager[currentlyAt].TurnSelectionEnd();

                hovered = currentlySelectedPosition;
                uiManager.GetTargetable(hovered).Selected();
                currentlySelectedToolManager = null;
                currentlySelectedPosition = null;
                currentlyAt = null;
            }
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

        public override void OnSelectUp()
        {
            SelectDirection(MoveDirection.Up);
        }

        public override void OnSubmit()
        {
            if (hovered != null)
            {
                currentlySelectedToolManager = manager.GetToolManager(hovered);
                currentlySelectedPosition = hovered;
                currentlyAt = hovered;
                uiManager.positionToManager[hovered].TurnSelectionStart();
                hovered = null;
            }
            else
            {
                hovered = currentlyAt;
                uiManager.positionToManager[hovered].TurnSelectionEnd();
                currentlySelectedToolManager = null;
                currentlySelectedPosition = null;
                currentlyAt = null;
            }
        }
    }
}
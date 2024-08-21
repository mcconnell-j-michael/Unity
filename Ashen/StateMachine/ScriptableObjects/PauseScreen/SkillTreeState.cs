using Ashen.AbilitySystem;
using Ashen.ControllerSystem;
using Ashen.NodeTreeSystem;
using Ashen.PauseSystem;
using Ashen.ToolSystem;
using Assets.Ashen.Pause.UI.Scripts;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.StateMachineSystem
{
    public class SkillTreeState : A_InputState, I_SkillTreePlayerInputListener
    {
        private ToolManager currentManager;
        private int currentManagerIdx;
        private List<ToolManager> allToolManagers;

        private NodeTreeUIManager nodeTreeUIManager;
        private PauseMenuPortraitManager portraitManager;
        private NodeUI currentNode;

        private I_TargetHolder targetHolder;

        public SkillTreeState(ToolManager defaultCharacter, I_TargetHolder targetHolder, I_Targetable defaultTargetable)
        {
            currentManager = defaultTargetable.GetTarget(); ;
            this.targetHolder = targetHolder;
            targetHolder.InitializeTarget(defaultTargetable);
        }

        protected override IEnumerator PreProcessState()
        {
            allToolManagers = new List<ToolManager>();
            portraitManager = PauseMenuManager.Instance.PauseMenuPortraitManager;
            A_PartyManager partyManager = PlayerPartyHolder.Instance.partyManager;
            nodeTreeUIManager = NodeTreeUIManager.Instance;
            int count = 0;
            currentManagerIdx = -1;
            int indexToSelect = -1;
            foreach (PartyPosition position in PartyPositions.Instance)
            {
                ToolManager toolManager = partyManager.GetToolManager(position);
                if (toolManager)
                {
                    allToolManagers.Add(toolManager);
                    if (!currentManager)
                    {
                        currentManager = toolManager;
                    }
                    if (currentManager == toolManager)
                    {
                        indexToSelect = count;
                    }
                    count += 1;
                }
            }
            SwitchCharacter(indexToSelect);
            yield break;
        }

        protected override void PostProcessState()
        {
            targetHolder.Cleanup();
        }

        public void OnSubmit()
        {
            if (IsDelayed(DelayGroup.GROUP_TWO))
            {
                return;
            }
            nodeTreeUIManager.OnClickNode(currentNode, currentNode.node);
            SetSelectDelay(0.15f, DelayGroup.GROUP_TWO);
        }

        public void OnCancel()
        {
            response.nextState = new ExitState();
        }

        public void OnSelectDown()
        {
            SelectNodeControl(currentNode.Down);
        }

        public void OnSelectLeft()
        {
            SelectNodeControl(currentNode.Left);
        }

        public void OnSelectRight()
        {
            SelectNodeControl(currentNode.Right);
        }

        public void OnSelectUp()
        {
            SelectNodeControl(currentNode.Up);
        }

        private void SelectNodeControl(NodeUI nextNode)
        {
            if (IsDelayed() || nextNode == null || nextNode == currentNode)
            {
                return;
            }
            SelectNode(nextNode);
        }

        private void SelectNode(NodeUI nextNode)
        {
            currentNode.Deselected();
            nextNode.Selected();
            currentNode = nextNode;
            SetSelectDelay(0.15f);
        }

        public void OnSwitchCharacterLeft()
        {
            SwitchCharacterControl(currentManagerIdx - 1);
        }

        public void OnSwitchCharacterRight()
        {
            SwitchCharacterControl(currentManagerIdx + 1);
        }

        private void SwitchCharacterControl(int newCharacterIdx)
        {
            if (IsDelayed())
            {
                return;
            }
            SwitchCharacter(newCharacterIdx);
        }

        private void SwitchCharacter(int newCharacterIdx)
        {
            if (newCharacterIdx < 0)
            {
                newCharacterIdx = allToolManagers.Count - 1;
            }
            if (newCharacterIdx >= allToolManagers.Count)
            {
                newCharacterIdx = 0;
            }
            if (currentManagerIdx == newCharacterIdx)
            {
                return;
            }
            currentManagerIdx = newCharacterIdx;
            currentManager = allToolManagers[currentManagerIdx];
            SkillTreeTool skillTreeTool = currentManager.Get<SkillTreeTool>();
            nodeTreeUIManager.RegisterSkillTree(skillTreeTool);
            PortraitTool portraitTool = currentManager.Get<PortraitTool>();
            portraitManager.UpdateSpriteImage(portraitTool.getPauseScreenPortrait());
            targetHolder.InitializeTarget(currentManager);
            foreach (List<NodeUI> nodeUis in nodeTreeUIManager.orderedNodeUis)
            {
                if (nodeUis.Count > 0)
                {
                    currentNode = nodeUis[0];
                    currentNode.Selected();
                    break;
                }
            }
            SetSelectDelay(0.15f);
        }

        protected override void RegisterInputManager()
        {
            SkillTreePlayerInputManager manager = SkillTreePlayerInputManager.Instance;
            manager.Enable();
            manager.RegisterListner(this);
        }

        protected override void UnRegisterInputManager()
        {
            SkillTreePlayerInputManager manager = SkillTreePlayerInputManager.Instance;
            manager.UnRegisterListener(this);
        }
    }
}
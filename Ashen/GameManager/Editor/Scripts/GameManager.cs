using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ashen.GameManagerWindow
{
    public class GameManager : OdinMenuEditorWindow
    {
        private int enumIndex = default;
        private bool treeRebuild = false;

        private int switchMenuCount = 1;

        private bool initialized = false;

        private string managerTitle;
        private string managerSubtitle;

        [OnValueChanged(nameof(StateChange))]
        [LabelText("Manager View")]
        //[ValueToggleButton("@" + nameof(gameManagerStates) + "." + nameof(GameManagerStateCollection.gameManagerStates))]
        [ValueToggleButton("@" + nameof(GameManager.BuildValueDropDown) + "()")]
        [ShowInInspector]
        private GameManagerState managerState = default;

        private A_GameManagerOption managerOption = default;
        private A_GameManagerOption previousManagerOption = default;

        protected GameManagerStateCollection gameManagerStates;
        protected List<A_GameManagerOption> gameManagerOptions;
        private GameManagerAssetOption selectedAssetManager;

        private List<ValueDropdownItem<GameManagerState>> BuildValueDropDown()
        {
            List<GameManagerState> states = gameManagerStates.gameManagerStates;
            List<ValueDropdownItem<GameManagerState>> valueStates = new List<ValueDropdownItem<GameManagerState>>();
            if (states != null)
            {
                foreach (GameManagerState state in states)
                {
                    ValueDropdownItem<GameManagerState> valueState = new ValueDropdownItem<GameManagerState>();
                    valueState.Value = state;
                    valueState.Text = state.ToString();
                    valueStates.Add(valueState);
                }
            }
            return valueStates;
        }

        protected virtual void StateChange()
        {
            switchMenuCount = 0;
            previousManagerOption.OnDeselected();
            treeRebuild = true;
            managerOption = gameManagerOptions[gameManagerStates.GetIndex(managerState)];
            managerOption.OnSelected();
            if (Is(GameManagerOptionType.ScriptableObjectAsset))
            {
                selectedAssetManager = managerOption as GameManagerAssetOption;
            }
            previousManagerOption = managerOption;
            MenuTree.Selection.Clear();
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (initialized)
            {
                Reinitialize(gameManagerStates, managerTitle, managerSubtitle);
            }
        }

        private void Reinitialize(GameManagerStateCollection gameManagerStates, string title, string subtitle)
        {
            Initialize(gameManagerStates, title, subtitle);
        }

        public void Initialize(GameManagerStateCollection gameManagerStates, string title, string subtitle)
        {
            initialized = true;
            initializeCount = 0;
            managerTitle = title;
            managerSubtitle = subtitle;
            this.gameManagerStates = gameManagerStates;
            gameManagerOptions = new List<A_GameManagerOption>();
            foreach (GameManagerState state in gameManagerStates.gameManagerStates)
            {
                A_GameManagerOption option = state.gameManagerOption.Clone(state);
                option.Initialize();
                gameManagerOptions.Add(option);
            }
            managerState = gameManagerStates.gameManagerStates[0];
            managerOption = gameManagerOptions[0];
            previousManagerOption = managerOption;
            managerOption.OnSelected();
            if (Is(GameManagerOptionType.ScriptableObjectAsset))
            {
                selectedAssetManager = managerOption as GameManagerAssetOption;
            }
        }

        private void Refresh(GameManagerStateCollection gameManagerStates, string title, string subtitle)
        {
            initialized = true;
            initializeCount = 0;
            managerTitle = title;
            managerSubtitle = subtitle;
            this.gameManagerStates = gameManagerStates;
            List<A_GameManagerOption> newGameManagerOptons = new List<A_GameManagerOption>();
            foreach (GameManagerState state in gameManagerStates.gameManagerStates)
            {
                A_GameManagerOption option = null;
                foreach (A_GameManagerOption oldOption in gameManagerOptions)
                {
                    if (oldOption.GetState() == state)
                    {
                        option = oldOption;
                        break;
                    }
                }
                if (option == null)
                {
                    option = state.gameManagerOption.Clone(state);
                    option.Initialize();
                }
                newGameManagerOptons.Add(option);
            }
            gameManagerOptions = newGameManagerOptons;
            if (!gameManagerStates.gameManagerStates.Contains(managerState))
            {
                managerState = gameManagerStates.gameManagerStates[0];
                if (managerOption != null)
                {
                    managerOption.OnDeselected();
                }
                managerOption = gameManagerOptions[0];
                previousManagerOption = managerOption;
                managerOption.OnSelected();
            }
            else
            {
                A_GameManagerOption newManagerOption = gameManagerOptions[gameManagerStates.GetIndex(managerState)];
                if (managerOption != newManagerOption)
                {
                    managerOption.OnDeselected();
                    managerOption = newManagerOption;
                    managerOption.OnSelected();
                }
            }
            if (Is(GameManagerOptionType.ScriptableObjectAsset))
            {
                selectedAssetManager = managerOption as GameManagerAssetOption;
            }
        }

        private bool GameManagerStatesChanged()
        {
            if (gameManagerOptions.Count != gameManagerStates.gameManagerStates.Count)
            {
                return true;
            }
            for (int x = 0; x < gameManagerOptions.Count; x++)
            {
                if (gameManagerStates.gameManagerStates[x] != gameManagerOptions[x].GetState())
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnImGUI()
        {
            if (Event.current.type == EventType.Layout && GameManagerStatesChanged())
            {
                Refresh(gameManagerStates, managerTitle, managerSubtitle);
            }

            if (treeRebuild && Event.current.type == EventType.Layout)
            {
                ForceMenuTreeRebuild();
                treeRebuild = false;
            }

            SirenixEditorGUI.Title(managerTitle, managerSubtitle, TextAlignment.Center, true);

            if (!Initial)
            {
                EditorGUILayout.Space();
                DrawEditor(enumIndex);
                EditorGUILayout.Space();
            }

            base.OnImGUI();
        }

        protected override void DrawEditors()
        {
            if (Initial)
            {
                DrawEditor(enumIndex);
                initializeCount++;
            }
            if (Is(GameManagerOptionType.ScriptableObjectAsset))
            {
                (managerOption as GameManagerAssetOption).SetSelected(MenuTree.Selection.SelectedValue);
            }
            if (!GameManagerStatesChanged())
            {
                DrawEditor(gameManagerStates.GetIndex(managerState));
            }
        }

        protected override IEnumerable<object> GetTargets()
        {
            List<object> targets = new List<object>();
            foreach (A_GameManagerOption option in gameManagerOptions)
            {
                option.AddTarget(targets);
            }

            targets.Add(base.GetTarget());

            enumIndex = targets.Count - 1;

            return targets;
        }

        protected override void DrawMenu()
        {
            if (switchMenuCount < 1)
            {
                switchMenuCount++;
            }
            else if (Is(GameManagerOptionType.ScriptableObjectAsset))
            {
                base.DrawMenu();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            managerOption.OnDeselected();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            if (Is(GameManagerOptionType.ScriptableObjectAsset))
            {
                selectedAssetManager.AddAllAssetsInPath(tree);
            }
            return tree;
        }

        private bool Initial
        {
            get
            {
                return initializeCount < 2;
            }
        }
        private int initializeCount;

        protected virtual bool Is(GameManagerOptionType gameManagerOptionType)
        {
            return managerOption.GetGameManagerOptionType() == gameManagerOptionType;
        }
    }
}

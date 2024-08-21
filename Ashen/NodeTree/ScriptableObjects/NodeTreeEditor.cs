using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.NodeTreeSystem
{
    public class NodeTreeEditor
    {
        [Hide]
        public NodeTree nodeTree;

        [Hide, EnumToggleButtons]
        public NodeTreeDirection direction;

        public List<List<NodeUIContainer>> Columns
        {
            get
            {
                if (nodeTree == null)
                {
                    nodeTree = new NodeTree();
                }
                return nodeTree.Columns;
            }
        }

        #region EDITOR
#if UNITY_EDITOR

        [TableMatrix(HorizontalTitle = "Columns", VerticalTitle = "Rows", DrawElementMethod = nameof(DrawColumnTest), SquareCells = true)]
        [OdinSerialize, OnInspectorGUI(Prepend = nameof(BeforeGUI)), PropertyOrder(0)]
        [HorizontalGroup("TreeEditor")]
        private NodeUIContainer[,] columnTest;


        private string toggleChoice = nameof(FocusedNode);
        [VerticalGroup("TreeEditor/Focus")]
        [ValueToggleButton("@" + nameof(BuildValueToggle) + "()"), Hide]
        [OdinSerialize]
        private string ToggleChoice
        {
            get
            {
                NodeUIContainer container = FocusedTest;
                if (container == null || container.type == NodeUIContainer.NodeUiType.Empty) { return nameof(FocusedNode); }
                if (toggleChoice == null)
                {
                    toggleChoice = nameof(FocusedNode);
                }
                return toggleChoice;
            }
            set
            {
                toggleChoice = value;
            }
        }

        [OdinSerialize, InlineEditor, ShowInInspector]
        [VerticalGroup("TreeEditor/Focus"), ShowIf("@" + nameof(ToggleChoice) + " == " + "\"" + nameof(FocusedNode) + "\"")]//nameof(ToggleChoice), nameof(FocusedNode))]
        private Node FocusedNode
        {
            get
            {
                if (FocusedTest == null)
                {
                    return null;
                }
                return FocusedTest.Node;
            }
            set
            {
                if (FocusedTest != null)
                {
                    if (value == null)
                    {
                        FocusedTest.type = NodeUIContainer.NodeUiType.Empty;
                    }
                    else
                    {
                        FocusedTest.type = NodeUIContainer.NodeUiType.SkillNode;
                        FocusedTest.node = value;
                    }
                }
            }
        }

        [OdinSerialize, InlineEditor, ShowInInspector]
        [VerticalGroup("TreeEditor/Focus"), ShowIf("@" + nameof(ToggleChoice) + " == " + "\"" + nameof(FocusedSkill) + "\"")]
        private A_NodeElement FocusedSkill
        {
            get
            {
                Node curNode = FocusedNode;
                if (curNode == null)
                {
                    return null;
                }
                return curNode.nodeElement;
            }
            set
            {
                Node curNode = FocusedNode;
                if (curNode != null)
                {
                    curNode.nodeElement = value;
                }
            }
        }

        private List<ValueDropdownItem<string>> BuildValueToggle()
        {
            List<ValueDropdownItem<string>> options = new()
            {
                new ValueDropdownItem<string>()
                {
                    Value = nameof(FocusedNode),
                    Text = "Node"
                },
            };
            NodeUIContainer container = FocusedTest;
            if (container == null || container.type == NodeUIContainer.NodeUiType.Empty) { return options; }
            options.Add(new ValueDropdownItem<string>()
            {
                Value = nameof(FocusedSkill),
                Text = "Skill"
            });
            return options;
        }

        private void BeforeGUI()
        {
            int cols = 0, rows = 0;
            int other = 0;
            int lowest = -1;
            int lowerDiffLimit = 1;
            int biggerDiffLimit = 1;
            List<NodeUIContainer> biggest = null;
            foreach (List<NodeUIContainer> containers in Columns)
            {
                int totalSize = 0;
                for (int x = 0; x < containers.Count; x++)
                {
                    NodeUIContainer container = containers[x];
                    totalSize += container.Space;
                }
                if (totalSize > other)
                {
                    biggerDiffLimit = 1;
                    other = totalSize;
                    biggest = containers;
                }
                if (other == totalSize)
                {
                    biggerDiffLimit -= 1;
                }
                if (lowest == -1)
                {
                    lowest = totalSize;
                }
                if (lowest != totalSize)
                {
                    lowerDiffLimit -= 1;
                }
                if (totalSize < lowest)
                {
                    lowest = totalSize;
                }
            }
            while (lowest != other && lowerDiffLimit >= 0 && biggerDiffLimit >= 0 && biggest != null)
            {
                NodeUIContainer last = biggest[biggest.Count - 1];
                if (last.type == NodeUIContainer.NodeUiType.Empty)
                {
                    biggest.RemoveAt(biggest.Count - 1);
                    other--;
                }
                else
                {
                    break;
                }
            }
            if (direction == NodeTreeDirection.LeftToRight)
            {
                cols = Columns.Count;
                rows = other;
            }
            else if (direction == NodeTreeDirection.BottomUp || direction == NodeTreeDirection.TopDown)
            {
                rows = Columns.Count;
                cols = other;
            }
            if (columnTest == null || columnTest.GetLength(0) != cols || columnTest.GetLength(1) != rows)
            {
                columnTest = new NodeUIContainer[cols, rows];
            }
            int colIdx = 0;
            int rowIdx = 0;
            for (int x = 0; x < Columns.Count; x++)
            {
                if (direction == NodeTreeDirection.LeftToRight)
                {
                    colIdx = x;
                    rowIdx = 0;
                }
                else if (direction == NodeTreeDirection.BottomUp)
                {
                    rowIdx = Columns.Count - 1 - x;
                    colIdx = 0;
                }
                else if (direction == NodeTreeDirection.TopDown)
                {
                    rowIdx = x;
                    colIdx = 0;
                }
                List<NodeUIContainer> containers = Columns[x];
                int totalSize = 0;
                for (int y = 0; y < containers.Count; y++)
                {
                    totalSize += containers[y].Space;
                }
                while (totalSize < other)
                {
                    containers.Add(new NodeUIContainer());
                    totalSize += 1;
                }
                for (int y = 0; y < containers.Count; y++)
                {
                    NodeUIContainer container = containers[y];
                    columnTest[colIdx, rowIdx] = container;
                    for (int z = 1; z < container.Space; z++)
                    {
                        if (direction == NodeTreeDirection.LeftToRight)
                        {
                            rowIdx += 1;
                        }
                        else if (direction == NodeTreeDirection.BottomUp || direction == NodeTreeDirection.TopDown)
                        {
                            colIdx += 1;
                        }
                        columnTest[colIdx, rowIdx] = null;
                    }
                    if (direction == NodeTreeDirection.LeftToRight)
                    {
                        rowIdx += 1;
                    }
                    else if (direction == NodeTreeDirection.BottomUp || direction == NodeTreeDirection.TopDown)
                    {
                        colIdx += 1;
                    }
                }
            }
        }

        private NodeUIContainer DrawColumnTest(Rect rect, NodeUIContainer value, int x, int y)
        {
            int topValue = 1;
            if (y == 0)
            {
                topValue = 2;
            }
            if (x >= columnTest.GetLength(0) || y >= columnTest.GetLength(1))
            {
                return value;
            }
            if (columnTest[x, y] == null)
            {
                bool mouseDown = false;
                if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                {
                    mouseDown = true;
                    GUI.changed = true;
                    Event.current.Use();
                }
                if (direction == NodeTreeDirection.LeftToRight)
                {
                    Color display = Color.cyan;
                    for (int searchY = y - 1; searchY >= 0; searchY--)
                    {
                        if (columnTest[x, searchY] != null)
                        {
                            if (focusedX == x && focusedY == searchY)
                            {
                                display = Color.white;
                            }
                            if (mouseDown)
                            {
                                focusedX = x;
                                focusedY = searchY;
                            }
                            break;
                        }
                    }
                    if ((y + 1) < columnTest.GetLength(1) && columnTest[x, y + 1] == null)
                    {
                        Sirenix.Utilities.Editor.SirenixEditorGUI.DrawBorders(rect, 1, 1, 0, 0, display);
                        return value;
                    }
                    Sirenix.Utilities.Editor.SirenixEditorGUI.DrawBorders(rect, 1, 1, 0, 1, display);
                    return value;
                }
                else if (direction == NodeTreeDirection.BottomUp || direction == NodeTreeDirection.TopDown)
                {
                    Color display = Color.cyan;
                    for (int searchX = x - 1; searchX >= 0; searchX--)
                    {
                        if (columnTest[searchX, y] != null)
                        {
                            if (focusedX == searchX && focusedY == y)
                            {
                                display = Color.white;
                            }
                            if (mouseDown)
                            {
                                focusedX = searchX;
                                focusedY = y;
                            }
                            break;
                        }
                    }
                    if ((x + 1) < columnTest.GetLength(0) && columnTest[x + 1, y] == null)
                    {
                        Sirenix.Utilities.Editor.SirenixEditorGUI.DrawBorders(rect, 0, 0, topValue, 1, display);
                        return value;
                    }
                    Sirenix.Utilities.Editor.SirenixEditorGUI.DrawBorders(rect, 0, 1, topValue, 1, display);
                    return value;
                }
            }

            if (value == null)
            {
                return value;
            }

            string message = "";

            if (value.type == NodeUIContainer.NodeUiType.SkillNode)
            {
                if (value.node == null)
                {
                    message = "NULL";
                }
                else
                {
                    message = value.node.displayName;
                }
            }

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                focusedX = x;
                focusedY = y;
                value ??= new NodeUIContainer();
                GUI.changed = true;
                Event.current.Use();
            }

            if (value != null && value.type == NodeUIContainer.NodeUiType.SkillNode && value.node != null && value.node.skillImage != null)
            {
                UnityEditor.EditorGUI.DrawPreviewTexture(rect, value.node.skillImage);
            }
            else
            {
                UnityEditor.EditorGUI.LabelField(rect, message, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
            }

            Color displayColor = Color.cyan;
            if (focusedX == x && focusedY == y)
            {
                displayColor = Color.white;
            }
            if (value.Space == 1)
            {
                Sirenix.Utilities.Editor.SirenixEditorGUI.DrawBorders(rect, 1, 1, topValue, 1, displayColor);
            }
            else
            {
                if (direction == NodeTreeDirection.LeftToRight)
                {
                    Sirenix.Utilities.Editor.SirenixEditorGUI.DrawBorders(rect, 1, 1, topValue, 0, displayColor);
                }
                else if (direction == NodeTreeDirection.BottomUp || direction == NodeTreeDirection.TopDown)
                {
                    Sirenix.Utilities.Editor.SirenixEditorGUI.DrawBorders(rect, 1, 0, topValue, 1, displayColor);
                }
            }

            return value;
        }

        private bool HideFocus()
        {
            if (columnTest == null)
            {
                return true;
            }
            if (focusedX < 0 || focusedX >= columnTest.GetLength(0))
            {
                return true;
            }
            if (focusedY < 0 || focusedY >= columnTest.GetLength(1))
            {
                return true;
            }
            return false;
        }
        private int focusedX = -1;
        private int focusedY = -1;

        [Button(ButtonHeight = 30), PropertyOrder(1)]
        [HorizontalGroup("Add")]
        public void AddRow()
        {
            if (Columns.Count == 0)
            {
                List<NodeUIContainer> containers = new List<NodeUIContainer>();
                containers.Add(new NodeUIContainer());
                Columns.Add(containers);
                return;
            }
            if (direction == NodeTreeDirection.LeftToRight)
            {
                AddRowInternal();
            }
            else if (direction == NodeTreeDirection.BottomUp || direction == NodeTreeDirection.TopDown)
            {
                AddColumnInternal();
            }
        }

        private void AddRowInternal()
        {
            foreach (List<NodeUIContainer> containers in Columns)
            {
                containers.Add(new NodeUIContainer());
            }
        }

        [Button(ButtonHeight = 30), PropertyOrder(1)]
        [HorizontalGroup("Add", Order = 1)]
        public void AddColumn()
        {
            if (Columns.Count == 0)
            {
                List<NodeUIContainer> containers = new List<NodeUIContainer>();
                containers.Add(new NodeUIContainer());
                Columns.Add(containers);
                return;
            }
            if (direction == NodeTreeDirection.LeftToRight)
            {
                AddColumnInternal();
            }
            else if (direction == NodeTreeDirection.BottomUp || direction == NodeTreeDirection.TopDown)
            {
                AddRowInternal();
            }
        }

        private void AddColumnInternal()
        {
            int total = 0;
            foreach (NodeUIContainer container in Columns[0])
            {
                total += container.Space;
            }
            List<NodeUIContainer> containers = new();
            for (int x = 0; x < total; x++)
            {
                containers.Add(new NodeUIContainer());
            }
            Columns.Add(containers);
        }

        [Button(ButtonHeight = 30), PropertyOrder(1)]
        [HorizontalGroup("Add")]
        public void TrimNodeTree()
        {
            int extraSpaceBottom = -1;
            for (int x = 0; x < Columns.Count; x++)
            {
                List<NodeUIContainer> containers = Columns[x];
                int curCount = 0;
                for (int y = containers.Count - 1; y >= 0; y--)
                {
                    NodeUIContainer container = containers[y];
                    if (container.type == NodeUIContainer.NodeUiType.SkillNode)
                    {
                        break;
                    }
                    else if (container.type == NodeUIContainer.NodeUiType.Empty)
                    {
                        curCount++;
                    }
                }
                if (extraSpaceBottom == -1 || extraSpaceBottom > curCount)
                {
                    extraSpaceBottom = curCount;
                }
            }
            for (int x = 0; x < Columns.Count; x++)
            {
                List<NodeUIContainer> containers = Columns[x];
                int toRemove = extraSpaceBottom;
                while (toRemove > 0)
                {
                    containers.RemoveAt(containers.Count - 1);
                    toRemove--;
                }
            }
            for (int x = Columns.Count - 1; x >= 0; x--)
            {
                List<NodeUIContainer> containers = Columns[x];
                bool remove = true;
                for (int y = 0; y < containers.Count; y++)
                {
                    NodeUIContainer container = containers[y];
                    if (container.type == NodeUIContainer.NodeUiType.SkillNode)
                    {
                        remove = false;
                    }
                }
                if (remove)
                {
                    Columns.RemoveAt(Columns.Count - 1);
                }
                else
                {
                    break;
                }
            }
        }

        [BoxGroup("Node", Order = 2)]
        [OdinSerialize, HideIf(nameof(HideFocus)), HideWithoutAutoPopulate, PropertyOrder(2)]
        private NodeUIContainer FocusedTest
        {
            get
            {
                if (HideFocus())
                {
                    return null;
                }
                return columnTest[focusedX, focusedY];
            }
            set
            {
                if (HideFocus())
                {
                    return;
                }
                columnTest[focusedX, focusedY] = value;
            }
        }



        private KeyValuePair<int, int> GetTrueIndex()
        {
            KeyValuePair<int, int> trueIndex = new(focusedX, focusedY);
            int colsIdx = focusedX;
            int colIdx = focusedY;
            if (direction == NodeTreeDirection.LeftToRight)
            {
                colsIdx = focusedX;
                colIdx = focusedY;
            }
            else if (direction == NodeTreeDirection.BottomUp)
            {
                colsIdx = Columns.Count - 1 - focusedY;
                colIdx = focusedX;
            }
            else if (direction == NodeTreeDirection.TopDown)
            {
                colsIdx = focusedY;
                colIdx = focusedX;
            }
            List<NodeUIContainer> column = Columns[colsIdx];
            int counter = colIdx;
            for (int x = 0; x < column.Count; x++)
            {
                if (counter <= 0)
                {
                    return new KeyValuePair<int, int>(colsIdx, x);
                }
                NodeUIContainer node = column[x];
                counter -= node.Space;
            }
            return new KeyValuePair<int, int>(focusedX, focusedY);
        }
        [Button, HorizontalGroup("Node/Move"), HideIf(nameof(HideFocus))]
        private void MoveUp()
        {
            if (direction == NodeTreeDirection.LeftToRight)
            {
                focusedY -= MoveUpInList();
            }
            else if (direction == NodeTreeDirection.BottomUp)
            {
                SetFocus(MoveRightList(focusedX));
            }
            else if (direction == NodeTreeDirection.TopDown)
            {
                SetFocus(MoveLeftList(focusedX));
            }
        }
        private int MoveUpInList()
        {
            KeyValuePair<int, int> trueIndex = GetTrueIndex();
            if (trueIndex.Value == 0)
            {
                return 0;
            }
            List<NodeUIContainer> column = Columns[trueIndex.Key];
            NodeUIContainer toMove = column[trueIndex.Value];
            column[trueIndex.Value] = column[trueIndex.Value - 1];
            column[trueIndex.Value - 1] = toMove;
            return column[trueIndex.Value].Space;
        }
        [Button, HorizontalGroup("Node/Move"), HideIf(nameof(HideFocus))]
        private void MoveDown()
        {
            if (direction == NodeTreeDirection.LeftToRight)
            {
                focusedY += MoveDownInList();
            }
            else if (direction == NodeTreeDirection.BottomUp)
            {
                SetFocus(MoveLeftList(focusedX));
            }
            else if (direction == NodeTreeDirection.TopDown)
            {
                SetFocus(MoveRightList(focusedX));
            }
        }
        private int MoveDownInList()
        {
            KeyValuePair<int, int> trueIndex = GetTrueIndex();
            if (trueIndex.Value == Columns[trueIndex.Key].Count - 1)
            {
                AddRowInternal();
            }
            List<NodeUIContainer> column = Columns[trueIndex.Key];
            NodeUIContainer toMove = column[trueIndex.Value];
            column[trueIndex.Value] = column[trueIndex.Value + 1];
            column[trueIndex.Value + 1] = toMove;
            return column[trueIndex.Value].Space;
        }
        private void SetFocus(KeyValuePair<int, int> newIdx)
        {
            int depth = 0;
            for (int x = 0; x < newIdx.Value; x++)
            {
                depth += Columns[newIdx.Key][x].Space;
            }
            if (direction == NodeTreeDirection.LeftToRight)
            {
                focusedX = newIdx.Key;
                focusedY = depth;
            }
            else if (direction == NodeTreeDirection.BottomUp)
            {
                focusedX = depth;
                focusedY = Columns.Count - 1 - newIdx.Key;
            }
            else if (direction == NodeTreeDirection.TopDown)
            {
                focusedX = depth;
                focusedY = newIdx.Key;
            }
        }
        [Button, HorizontalGroup("Node/Move"), HideIf(nameof(HideFocus))]
        private void MoveLeft()
        {
            if (direction == NodeTreeDirection.LeftToRight)
            {
                SetFocus(MoveLeftList(focusedY));
            }
            else if (direction == NodeTreeDirection.BottomUp)
            {
                focusedX -= MoveUpInList();
            }
            else if (direction == NodeTreeDirection.TopDown)
            {
                focusedX -= MoveUpInList();
            }
        }
        private KeyValuePair<int, int> MoveLeftList(int virtualDepth)
        {
            KeyValuePair<int, int> trueIndex = GetTrueIndex();
            if (trueIndex.Key == 0)
            {
                return trueIndex;
            }
            List<NodeUIContainer> sourceList = Columns[trueIndex.Key];
            List<NodeUIContainer> toList = Columns[trueIndex.Key - 1];
            return new KeyValuePair<int, int>(trueIndex.Key - 1, SwapHorizontal(sourceList, toList, trueIndex.Value, virtualDepth));
        }
        [Button, HorizontalGroup("Node/Move"), HideIf(nameof(HideFocus))]
        private void MoveRight()
        {
            if (direction == NodeTreeDirection.LeftToRight)
            {
                SetFocus(MoveRightList(focusedY));
            }
            else if (direction == NodeTreeDirection.BottomUp)
            {
                focusedX += MoveDownInList();
            }
            else if (direction == NodeTreeDirection.TopDown)
            {
                focusedX += MoveDownInList();
            }
        }
        private KeyValuePair<int, int> MoveRightList(int virtualDepth)
        {
            KeyValuePair<int, int> trueIndex = GetTrueIndex();
            if (trueIndex.Key == Columns.Count - 1)
            {
                AddColumnInternal();
            }
            List<NodeUIContainer> sourceList = Columns[trueIndex.Key];
            List<NodeUIContainer> toList = Columns[trueIndex.Key + 1];
            return new KeyValuePair<int, int>(trueIndex.Key + 1, SwapHorizontal(sourceList, toList, trueIndex.Value, virtualDepth));
        }

        private int SwapHorizontal(List<NodeUIContainer> sourceList, List<NodeUIContainer> toList, int sourceIdx, int virtualDepth)
        {
            NodeUIContainer toSwap = sourceList[sourceIdx];
            int fromStartIdx = sourceIdx;
            int fromEndIdx = sourceIdx;
            int sourceDepthStart = virtualDepth;
            int sourceDepthEnd = virtualDepth + sourceList[sourceIdx].Space;
            int toDepthStart = -1;
            int toStartIdx = -1;
            int toDepthEnd = -1;
            int toEndIdx = -1;
            int depthSearch = 0;
            for (int x = 0; x < toList.Count; x++)
            {
                if (toDepthStart == -1 && depthSearch + toList[x].Space > sourceDepthStart)
                {
                    toDepthStart = depthSearch;
                    toStartIdx = x;
                }
                if (toDepthEnd == -1 && depthSearch + toList[x].Space >= sourceDepthEnd)
                {
                    toDepthEnd = depthSearch + toList[x].Space;
                    toEndIdx = x;
                }
                depthSearch += toList[x].Space;
            }

            bool canFullSwap = true;
            int newFromStartIdx = fromStartIdx;
            int newSourceDepthStart = sourceDepthStart;
            int newFromEndIdx = fromEndIdx;
            int newSourceDepthEnd = sourceDepthEnd;
            while (canFullSwap && newSourceDepthStart > toDepthStart)
            {
                if (sourceList[newFromStartIdx - 1].type != NodeUIContainer.NodeUiType.Empty)
                {
                    canFullSwap = false;
                    break;
                }
                newFromStartIdx--;
                newSourceDepthStart--;
            }

            while (canFullSwap && newSourceDepthEnd < toDepthEnd)
            {
                if (sourceList[newFromEndIdx + 1].type != NodeUIContainer.NodeUiType.Empty)
                {
                    canFullSwap = false;
                    break;
                }
                newFromEndIdx++;
                newSourceDepthEnd++;
            }

            if (canFullSwap)
            {
                fromStartIdx = newFromStartIdx;
                sourceDepthStart = newSourceDepthStart;
                fromEndIdx = newFromEndIdx;
                sourceDepthEnd = newSourceDepthEnd;

                for (int x = fromEndIdx; x >= fromStartIdx; x--)
                {
                    toList.Insert(toEndIdx + 1, sourceList[x]);
                    sourceList.RemoveAt(x);
                }
                for (int x = toEndIdx; x >= toStartIdx; x--)
                {
                    sourceList.Insert(fromStartIdx, toList[x]);
                    toList.RemoveAt(x);
                }
            }
            else
            {
                for (int x = toStartIdx; x <= toEndIdx; x++)
                {
                    if (toList[x].type == NodeUIContainer.NodeUiType.SkillNode)
                    {
                        NodeUIContainer toNode = toList[x];
                        sourceList[fromStartIdx] = toNode;
                        int toSpace = toNode.Space;
                        toNode.Space = toSwap.Space;
                        toSwap.Space = toSpace;
                        toList[x] = toSwap;
                        break;
                    }
                }
            }

            for (int x = 0; x < toList.Count; x++)
            {
                if (toList[x] == toSwap)
                {
                    return x;
                }
            }

            return -1;
        }
#endif
        #endregion

    }
}
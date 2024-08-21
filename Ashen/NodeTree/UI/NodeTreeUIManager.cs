using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ashen.NodeTreeSystem
{
    public class NodeTreeUIManager : SingletonMonoBehaviour<NodeTreeUIManager>
    {
        [ShowInInspector]
        private I_NodeTreeManager nodeTreeManager;
        [InlineEditor]
        public List<NodeUI> nodeUIs;

        public ScrollRect scrollRectViewer;

        [FoldoutGroup("Valid Node"), AutoPopulate, HideLabel]
        public SkillTreeNodeColorScheme validNode;
        [FoldoutGroup("Invalid Node"), AutoPopulate, HideLabel]
        public SkillTreeNodeColorScheme invalidNode;
        public Color selectedNodeColor;

        public TextMeshProUGUI totalPoints;
        public TextMeshProUGUI partyMemberName;
        public TextMeshProUGUI className;

        public Color selectedIndicator;
        public Color inactiveIndicator;

        public List<NodeTreeColumnUI> columns;
        public List<NodeTreeColumnListUI> columnContainers;

        public GameObject columnContainerHolder;
        public GameObject columnContainerPrefab;

        public GameObject linesHolder;
        public GameObject linePrefab;
        public GameObject lineContainerPrefab;

        public GameObject requirementsHolder;
        public GameObject requirementsPrefab;

        public GameObject nodePrefab;
        public GameObject nodeContainerPrefab;

        public List<GameObject> containers;
        public List<GameObject> lines;
        public List<GameObject> requirements;

        public Dictionary<Node, List<NodeRequirementsPositionController>> requirementControllers;
        public Dictionary<Node, GameObject> nodeToLineContainer;

        public List<List<NodeUI>> orderedNodeUis;

        public void RegisterSkillTree(I_NodeTreeManager nodeTreeManager)
        {
            this.nodeTreeManager = nodeTreeManager;
            this.partyMemberName.text = nodeTreeManager.GetName();
            LoadNodeTree(nodeTreeManager.GetNodeTreeDefinition());
            UpdateNodes();
        }

        public void RegisterNode(NodeUI node, bool batchUpdate = false)
        {
            if (nodeUIs == null)
            {
                nodeUIs = new List<NodeUI>();
            }
            if (!nodeUIs.Contains(node))
            {
                nodeUIs.Add(node);
            }
            if (!batchUpdate)
            {
                UpdateNodes();
            }
        }

        public void OnClickNode(NodeUI nodeUI, Node node)
        {
            if (nodeTreeManager == null)
            {
                return;
            }
            NodeIncreaseRequestResponse response = nodeTreeManager.RequestSkillNodeIncrease(node);
            if (response == NodeIncreaseRequestResponse.MAX_LEVEL || response == NodeIncreaseRequestResponse.SUCCESS)
            {
                int skilLevel = nodeTreeManager.GetCurrentLevel(node);
                if (skilLevel == 1)
                {
                    nodeUI.OnFirstNodePoint();
                }
            }
            UpdateNodes();
        }

        public void UpdateScreenPosition(int column)
        {
            int currentColumn = -1;
            int listIndex = -1;
            for (int x = 0; x < columnContainers.Count; x++)
            {
                currentColumn += columnContainers[x].columns.Count;
                if (currentColumn >= column)
                {
                    listIndex = x;
                    break;
                }
            }
            float interval = 0f;
            if (columnContainers.Count > 1)
            {
                interval = 1f / (columnContainers.Count);
            }
            if (listIndex == 0)
            {
                scrollRectViewer.horizontalNormalizedPosition = 0f;
            }
            //else if (listIndex == columnContainers.Count - 1)
            //{
            //    scrollRectViewer.horizontalNormalizedPosition = 1f;
            //}
            else
            {
                scrollRectViewer.horizontalNormalizedPosition = (listIndex * interval);
            }
        }

        public void OnClickRight(NodeUI nodeUI, Node node)
        {
        }

        [Button]
        public void UpdateNodes()
        {
            if (nodeUIs == null)
            {
                return;
            }
            if (nodeTreeManager != null)
            {
                totalPoints.text = nodeTreeManager.GetTotalPoints().ToString();
            }
            foreach (NodeUI nodeUI in nodeUIs)
            {
                if (nodeTreeManager == null)
                {
                    if (nodeUI.node.hasRequirements && nodeUI.node.requirements?.Count > 0)
                    {
                        nodeUI.OnDisabledNode();
                    }
                    else
                    {
                        nodeUI.OnValidOption();
                    }
                    continue;
                }
                if (nodeTreeManager.RequirementsMet(nodeUI.node))
                {

                }
                if (nodeTreeManager.GetCurrentLevel(nodeUI.node) == 0)
                {
                    if (!nodeTreeManager.CanIncreaseSkillNode(nodeUI.node))
                    {
                        nodeUI.OnDisabledNode();
                    }
                    else
                    {
                        if (nodeTreeManager.GetTotalPoints() > 0)
                        {
                            nodeUI.OnValidOption();
                        }
                        else
                        {
                            nodeUI.OnMissingPoints();
                        }
                    }
                    nodeUI.SetTierLevel(nodeTreeManager.GetTierLevel(nodeUI.node));
                }
                else
                {
                    if (nodeTreeManager.CanIncreaseSkillNode(nodeUI.node))
                    {
                        if (nodeTreeManager.GetTotalPoints() > 0)
                        {
                            nodeUI.OnValidOption();
                        }
                        else
                        {
                            nodeUI.OnMissingPoints();
                        }
                    }
                    else if (nodeTreeManager.IsNodeMax(nodeUI.node))
                    {
                        nodeUI.OnMaxNode();
                    }
                }
                if (nodeUI.node.hasRequirements)
                {
                    if (requirementControllers.TryGetValue(nodeUI.node, out List<NodeRequirementsPositionController> reqControllers))
                    {
                        foreach (NodeRequirementsPositionController controller in reqControllers)
                        {
                            bool toDisable = true;
                            foreach (I_NodeRequirements nodeReq in controller.nodeRequirements)
                            {
                                if (!nodeReq.RequirementsMet(nodeTreeManager))
                                {
                                    toDisable = false;
                                    break;
                                }
                            }
                            controller.disable = toDisable;
                        }
                    }
                }
                if (nodeToLineContainer.TryGetValue(nodeUI.node, out GameObject lineContainer))
                {
                    for (int x = 0; x < lineContainer.transform.childCount; x++)
                    {
                        GameObject lineChild = lineContainer.transform.GetChild(x).gameObject;
                        NodeSquareLineDrawerUI drawer = lineChild.GetComponent<NodeSquareLineDrawerUI>();

                        OtherNodeRequirement otherRequirement = drawer.requirement as OtherNodeRequirement;
                        if (drawer.requirement.RequirementsMet(nodeTreeManager))
                        {
                            drawer.Enable();
                            if (otherRequirement == null || !otherRequirement.enabledOnTop)
                            {
                                lineChild.transform.SetAsFirstSibling();
                            }
                        }
                        else
                        {

                            drawer.Disable();
                            if (otherRequirement != null && otherRequirement.enabledOnTop)
                            {
                                lineChild.transform.SetAsFirstSibling();
                            }
                        }
                    }
                }
                nodeUI.currentValueText.text = nodeTreeManager.GetCurrentLevel(nodeUI.node) + "";
                nodeUI.SetTierLevel(nodeTreeManager.GetTierLevel(nodeUI.node));
#if UNITY_EDITOR
                EditorUtility.SetDirty(nodeUI.currentValueText);
#endif
            }
        }

        [Button]
        private void LoadNodeTree(List<List<NodeUIContainer>> nodeTreeDefinition)
        {
            if (nodeUIs == null)
            {
                nodeUIs = new List<NodeUI>();
            }
            nodeUIs.Clear();
            if (containers == null)
            {
                containers = new List<GameObject>();
            }
            foreach (GameObject container in containers)
            {
                DestroyUtils.Destroy(container);
            }
            containers.Clear();
            if (lines == null)
            {
                lines = new List<GameObject>();
            }
            foreach (GameObject line in lines)
            {
                DestroyUtils.Destroy(line);
            }
            lines.Clear();
            if (requirements == null)
            {
                requirements = new List<GameObject>();
            }
            foreach (GameObject requirement in requirements)
            {
                DestroyUtils.Destroy(requirement);
            }
            requirements.Clear();
            if (requirementControllers == null)
            {
                requirementControllers = new Dictionary<Node, List<NodeRequirementsPositionController>>();
            }
            requirementControllers.Clear();
            if (nodeToLineContainer == null)
            {
                nodeToLineContainer = new Dictionary<Node, GameObject>();
            }
            if (orderedNodeUis == null)
            {
                orderedNodeUis = new List<List<NodeUI>>();
            }
            orderedNodeUis.Clear();
            if (columnContainers == null)
            {
                columnContainers = new List<NodeTreeColumnListUI>();
            }
            foreach (NodeTreeColumnListUI nodeTreeColumnListUI in columnContainers)
            {
                if (nodeTreeColumnListUI)
                {
                    DestroyUtils.Destroy(nodeTreeColumnListUI.gameObject);
                }
            }
            columnContainers.Clear();
            columns.Clear();
            foreach (KeyValuePair<Node, GameObject> pair in nodeToLineContainer)
            {
                DestroyUtils.Destroy(pair.Value);
            }
            nodeToLineContainer.Clear();
            List<KeyValuePair<Node, float>>[] orderedSkills = new List<KeyValuePair<Node, float>>[6];
            for (int x = 0; x < orderedSkills.Length; x++)
            {
                orderedSkills[x] = new List<KeyValuePair<Node, float>>();
            }
            Dictionary<Node, NodeUI> nodeConfigToElement = new Dictionary<Node, NodeUI>();
            //List<NodeUI> nodeUis = new List<NodeUI>();
            //orderedNodeUis.Add(nodeUis);
            GameObject lastColumnContainer = null;
            int curColumn = 0;
            foreach (List<NodeUIContainer> nodeUIColumnContainers in nodeTreeDefinition)
            {
                if (this.columns.Count <= curColumn)
                {
                    GameObject columnContainer = Instantiate(columnContainerPrefab, columnContainerHolder.transform);
                    if (lastColumnContainer != null)
                    {
                        RectTransform lastColumnRectTransform = lastColumnContainer.transform as RectTransform;
                        RectTransform currentTransform = columnContainer.transform as RectTransform;
                        float lastRectWidth = lastColumnRectTransform.rect.width;
                        float lastPosX = lastColumnRectTransform.localPosition.x;
                        float xPos = lastColumnRectTransform.localPosition.x + lastColumnRectTransform.rect.width;
                        currentTransform.localPosition += new Vector3(lastColumnRectTransform.localPosition.x + lastColumnRectTransform.rect.width, 0, 0);
                    }
                    lastColumnContainer = columnContainer;
                    NodeTreeColumnListUI columnList = columnContainer.GetComponent<NodeTreeColumnListUI>();
                    columnContainers.Add(columnList);
                    List<NodeTreeColumnUI> columns = columnList.columns;
                    this.columns.AddRange(columns);
                    //foreach (KeyValuePair<Node, float> pair in orderedSkills[0])
                    //{
                    //    nodeUis.Add(nodeConfigToElement[pair.Key]);
                    //}
                    //nodeUIs = new List<NodeUI>();
                    //orderedNodeUis.Add(nodeUIs);
                }
                LoadSKillsForColumn(nodeUIColumnContainers, this.columns[curColumn].gameObject, nodeConfigToElement, orderedSkills[curColumn], curColumn, this.columns[curColumn].widthMultiplier);
                curColumn++;
            }
            GenerateNavigation(nodeConfigToElement, orderedSkills);
            GenerateLines(nodeConfigToElement);
            UpdateNodes();
        }

        private void LoadSKillsForColumn(List<NodeUIContainer> nodeUIContainers, GameObject column, Dictionary<Node, NodeUI> nodeConfigToElement, List<KeyValuePair<Node, float>> skillOrder, int columnNum, float widthMultiplier)
        {
            RectTransform referenceRect = (column.transform as RectTransform);
            float oneUnitHeight = referenceRect.rect.height / 7f;
            float width = referenceRect.rect.width;
            float height = referenceRect.rect.height;

            int usedSections = 0;
            List<NodeUI> orderedNodes = new();
            orderedNodeUis.Add(orderedNodes);
            foreach (NodeUIContainer nodeUIContainer in nodeUIContainers)
            {
                if (usedSections + nodeUIContainer.Space > 7)
                {
                    break;
                }
                GameObject container = Instantiate(nodeContainerPrefab, column.transform);
                containers.Add(container);
                RectTransform containerRect = container.transform as RectTransform;
                containerRect.localPosition = new Vector3(widthMultiplier * width / 2f, (-usedSections * oneUnitHeight) + (height / 2f), 0);
                containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, oneUnitHeight * nodeUIContainer.Space);
                if (nodeUIContainer.type == NodeUIContainer.NodeUiType.SkillNode)
                {
                    GameObject nodeGO = Instantiate(nodePrefab, container.transform);
                    NodeUI nodeUI = nodeGO.GetComponent<NodeUI>();
                    nodeUI.column = columnNum;
                    nodeUI.RegisterNode(nodeUIContainer.node);
                    nodeConfigToElement.Add(nodeUIContainer.node, nodeUI);
                    nodeUI.nodeTreeUI = this;
                    RegisterNode(nodeUI, true);
                    KeyValuePair<Node, float> pair = new KeyValuePair<Node, float>(nodeUIContainer.node, usedSections + ((nodeUIContainer.Space - 1f) / 2f));
                    skillOrder.Add(pair);
                    nodeUIs.Add(nodeUI);
                    orderedNodes.Add(nodeUI);
                }
                usedSections += nodeUIContainer.Space;
            }
        }

        private void GenerateNavigation(Dictionary<Node, NodeUI> nodeConfiguration, List<KeyValuePair<Node, float>>[] orderedSkills)
        {
            foreach (KeyValuePair<Node, NodeUI> pair in nodeConfiguration)
            {
                Node node = pair.Key;
                NodeUI nodeUi = pair.Value;

                int column = nodeUi.column;
                int foundIndex = -1;
                List<KeyValuePair<Node, float>> columnList = orderedSkills[column];
                for (int x = 0; x < columnList.Count; x++)
                {
                    if (orderedSkills[column][x].Key == node)
                    {
                        foundIndex = x;
                        break;
                    }
                }


                // Find up navigation
                if (columnList.Count > 1)
                {
                    if (foundIndex == 0)
                    {
                        nodeUi.Up = nodeConfiguration[columnList[columnList.Count - 1].Key];
                    }
                    else
                    {
                        nodeUi.Up = nodeConfiguration[columnList[foundIndex - 1].Key];
                    }
                }
                else
                {
                    List<KeyValuePair<Node, float>> otherList = null;
                    if (column % 2 == 0)
                    {
                        otherList = orderedSkills[column + 1];
                    }
                    else
                    {
                        otherList = orderedSkills[column - 1];
                    }
                    if (otherList.Count > 0)
                    {
                        nodeUi.Up = nodeConfiguration[otherList[0].Key];
                    }
                }

                // Find down navigation
                if (orderedSkills[column].Count > 1)
                {
                    if (foundIndex == columnList.Count - 1)
                    {
                        nodeUi.Down = nodeConfiguration[columnList[0].Key];
                    }
                    else
                    {
                        nodeUi.Down = nodeConfiguration[columnList[foundIndex + 1].Key];
                    }
                }
                else
                {
                    List<KeyValuePair<Node, float>> otherList = null;
                    if (column % 2 == 0)
                    {
                        otherList = orderedSkills[column + 1];
                    }
                    else
                    {
                        otherList = orderedSkills[column - 1];
                    }
                    if (otherList.Count > 0)
                    {
                        nodeUi.Down = nodeConfiguration[otherList[otherList.Count - 1].Key];
                    }
                }

                // Find left navigation
                KeyValuePair<Node, float>? left = FindBestFitLeft(columnList[foundIndex], column, orderedSkills);
                if (left != null)
                {
                    nodeUi.Left = nodeConfiguration[left.Value.Key];
                }

                // Find right navigation
                KeyValuePair<Node, float>? right = FindBestFitRight(columnList[foundIndex], column, orderedSkills);
                if (right != null)
                {
                    nodeUi.Right = nodeConfiguration[right.Value.Key];
                }
            }
        }

        private KeyValuePair<Node, float>? FindBestFitLeft(KeyValuePair<Node, float> source, int column, List<KeyValuePair<Node, float>>[] orderedSkills)
        {
            if (column == 0)
            {
                return null;
            }
            KeyValuePair<Node, float>? closest = null;
            KeyValuePair<Node, float> next = FindClosestFit(source, orderedSkills[column - 1]);
            if (next.Key != null)
            {
                closest = next;
                if (Math.Abs(source.Value - next.Value) < .6f)
                {
                    return next;
                }
            }
            if (column - 1 == 0)
            {
                return closest;
            }
            KeyValuePair<Node, float> twoOver = FindClosestFit(source, orderedSkills[column - 2]);
            if (twoOver.Key != null)
            {
                if (closest == null || Math.Abs(twoOver.Value - source.Value) < Math.Abs(closest.Value.Value - source.Value))
                {
                    closest = twoOver;
                }
            }
            if (closest != null)
            {
                return closest;
            }
            for (int x = column - 3; x > 0; x--)
            {
                KeyValuePair<Node, float> found = FindClosestFit(source, orderedSkills[x]);
                if (found.Key != null)
                {
                    return found;
                }
            }
            return closest;
        }

        private KeyValuePair<Node, float>? FindBestFitRight(KeyValuePair<Node, float> source, int column, List<KeyValuePair<Node, float>>[] orderedSkills)
        {
            if (column == orderedSkills.Length - 1)
            {
                return null;
            }
            KeyValuePair<Node, float>? closest = null;
            KeyValuePair<Node, float> next = FindClosestFit(source, orderedSkills[column + 1]);
            if (next.Key != null)
            {
                closest = next;
                if (Math.Abs(source.Value - next.Value) < .6f)
                {
                    return next;
                }
            }
            if (column + 1 == orderedSkills.Length - 1)
            {
                return closest;
            }
            KeyValuePair<Node, float> twoOver = FindClosestFit(source, orderedSkills[column + 2]);
            if (twoOver.Key != null)
            {
                if (closest == null || Math.Abs(twoOver.Value - source.Value) < Math.Abs(closest.Value.Value - source.Value))
                {
                    closest = twoOver;
                }
            }
            if (closest != null)
            {
                return closest;
            }
            for (int x = column + 3; x < orderedSkills.Length; x++)
            {
                KeyValuePair<Node, float> found = FindClosestFit(source, orderedSkills[x]);
                if (found.Key != null)
                {
                    return found;
                }
            }
            return closest;
        }

        private KeyValuePair<Node, float> FindClosestFit(KeyValuePair<Node, float> source, List<KeyValuePair<Node, float>> targetList)
        {
            KeyValuePair<Node, float> closest = new KeyValuePair<Node, float>();
            foreach (KeyValuePair<Node, float> other in targetList)
            {
                if (closest.Key == null || Math.Abs(source.Value - other.Value) < Math.Abs(source.Value - closest.Value))
                {
                    closest = other;
                }
            }
            return closest;
        }

        private void GenerateLines(Dictionary<Node, NodeUI> nodeConfigToElement)
        {
            foreach (KeyValuePair<Node, NodeUI> pair in nodeConfigToElement)
            {
                Node node = pair.Key;
                if (node.hasRequirements && node.requirements?.Count > 0)
                {
                    foreach (I_NodeRequirements requirements in node.requirements)
                    {
                        if (!(requirements is OtherNodeRequirement otherNodeRequirement) || !nodeConfigToElement.ContainsKey(otherNodeRequirement.node))
                        {
                            continue;
                        }
                        GameObject lineHolder = null;
                        Node owner = node;
                        if (otherNodeRequirement.inverseLineOwner)
                        {
                            owner = otherNodeRequirement.node;
                        }
                        if (nodeToLineContainer.TryGetValue(owner, out GameObject lineContainer))
                        {
                            lineHolder = Instantiate(linePrefab, lineContainer.transform);
                        }
                        else
                        {
                            lineContainer = Instantiate(lineContainerPrefab, linesHolder.transform);
                            lineHolder = Instantiate(linePrefab, lineContainer.transform);
                            nodeToLineContainer.Add(owner, lineContainer);
                        }
                        lines.Add(lineHolder);
                        NodeSquareLineDrawerUI drawer = lineHolder.GetComponent<NodeSquareLineDrawerUI>();
                        drawer.requirement = requirements;
                        drawer.reference = transform;
                        drawer.locations = new List<UILocation>();
                        if (otherNodeRequirement.lineConfigurations?.Count > 0)
                        {
                            foreach (NodeLineConfiguration config in otherNodeRequirement.lineConfigurations)
                            {
                                drawer.locations.Add(new UILocation()
                                {
                                    rectTransform = nodeConfigToElement[config.node].transform as RectTransform,
                                    bothResolver = config.configuration.bothResolver,
                                    coord = config.configuration.coord,
                                    rectSide = config.configuration.rectSide,
                                    splitPercentage = config.configuration.splitPercentage,
                                });
                            }
                        }
                        else
                        {
                            drawer.locations.Add(new UILocation()
                            {
                                rectTransform = nodeConfigToElement[otherNodeRequirement.node].transform as RectTransform,
                                rectSide = RectSide.RIGHT,
                                coord = Coord.X,
                            });
                            drawer.locations.Add(new UILocation()
                            {
                                rectTransform = pair.Value.transform as RectTransform,
                                rectSide = RectSide.LEFT,
                                coord = Coord.BOTH,
                                bothResolver = BothResolver.MIDXY,
                                splitPercentage = 50,
                            });
                        }
                    }

                    List<NodeRequirementsConfiguration> configurations = node.skilNodeRequirements;
                    List<OtherNodeRequirement> evalutatedNodeRequirements = new List<OtherNodeRequirement>();
                    int count = 1;
                    foreach (NodeRequirementsConfiguration configuration in configurations)
                    {
                        List<OtherNodeRequirement> requirementsToBaseOn = new List<OtherNodeRequirement>();
                        OtherNodeRequirement requirementToUseAsRequiresReference = null;
                        if (node.requirements.Count == 1)
                        {
                            if (!(node.requirements[0] is OtherNodeRequirement))
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " is attempting to use requirement that is not of type '" + nameof(OtherNodeRequirement) + "'");
                                continue;
                            }
                            OtherNodeRequirement otherNodeRequirement = node.requirements[0] as OtherNodeRequirement;
                            if (evalutatedNodeRequirements.Contains(otherNodeRequirement))
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " defined requirement displays using the same requirements");
                                continue;
                            }
                            requirementToUseAsRequiresReference = otherNodeRequirement;
                            requirementsToBaseOn.Add(otherNodeRequirement);
                        }
                        else
                        {
                            foreach (int index in configuration.indexesOfRequirementsToBaseOn)
                            {
                                if (!(node.requirements[index] is OtherNodeRequirement requirements))
                                {
                                    Logger.ErrorLog("SkillNode: " + node.name + " is attempting to use requirement that is not of type '" + nameof(OtherNodeRequirement) + "'");
                                    continue;
                                }
                                if (evalutatedNodeRequirements.Contains(requirements))
                                {
                                    Logger.ErrorLog("SkillNode: " + node.name + " defined requirement displays using the same requirements");
                                    continue;
                                }
                                if (!requirementsToBaseOn.Contains(requirements))
                                {
                                    requirementsToBaseOn.Add(requirements);
                                }
                            }
                            if (configuration.indexesOfRequirementsToBaseOn.Count == 1)
                            {
                                requirementToUseAsRequiresReference = node.requirements[configuration.indexesOfRequirementsToBaseOn[0]] as OtherNodeRequirement;
                            }
                            else
                            {
                                requirementToUseAsRequiresReference = node.requirements[configuration.indexToBaseOn] as OtherNodeRequirement;
                            }
                            if (requirementToUseAsRequiresReference == null)
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " is attempting to use requirement that is not of type '" + nameof(OtherNodeRequirement) + "'");
                                requirementToUseAsRequiresReference = requirementsToBaseOn[0];
                            }
                            if (!requirementsToBaseOn.Contains(requirementToUseAsRequiresReference))
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " defined requirement to base on that is not an initial requirement");
                                requirementToUseAsRequiresReference = requirementsToBaseOn[0];
                            }
                        }
                        int requirementLevelForRequires = requirementToUseAsRequiresReference.levelRequired;
                        for (int x = 0; x < requirementsToBaseOn.Count; x++)
                        {
                            if (requirementsToBaseOn[x].levelRequired != requirementLevelForRequires)
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " is attempting to combine two requirements with different requirement levels");
                                requirementsToBaseOn.RemoveAt(x);
                                x--;
                            }
                        }
                        GameObject requirementGO = Instantiate(requirementsPrefab, requirementsHolder.transform);
                        requirements.Add(requirementGO);
                        NodeRequirementsPositionController controller = requirementGO.GetComponent<NodeRequirementsPositionController>();
                        controller.reference = transform;
                        List<NodeUI> nodeUIs = new List<NodeUI>();
                        List<I_NodeRequirements> baseRequirements = new List<I_NodeRequirements>();
                        foreach (OtherNodeRequirement other in requirementsToBaseOn)
                        {
                            if (!nodeConfigToElement.ContainsKey(other.node))
                            {
                                continue;
                            }
                            nodeUIs.Add(nodeConfigToElement[other.node]);
                            baseRequirements.Add(other);
                        }
                        if (nodeUIs.Count == 0)
                        {
                            continue;
                        }
                        if (requirementControllers.TryGetValue(node, out List<NodeRequirementsPositionController> controllers))
                        {
                            controllers.Add(controller);
                        }
                        else
                        {
                            requirementControllers.Add(node, new List<NodeRequirementsPositionController>() { controller });
                        }
                        NodeUI focusUi = nodeConfigToElement[requirementToUseAsRequiresReference.node];
                        controller.Initialize(baseRequirements, nodeUIs, focusUi, nodeConfigToElement[node], requirementToUseAsRequiresReference.levelRequired);
                        controller.locationX = configuration.locationX;
                        controller.locationY = configuration.locationY;
                        controller.requiresBound = configuration.requiresBound;
                        controller.sourceBound = configuration.sourceBound;
                        controller.gameObject.name = node.name + " " + count;
                        count++;
                    }
                }

                if (node.presentsRequirements && node.skillNodePresentsRequirements?.Count > 0)
                {
                    List<Node> evaluatedSkillNodes = new List<Node>();
                    int count = 1;
                    foreach (NodePresentsRequirementsConfiguration configuration in node.skillNodePresentsRequirements)
                    {
                        List<Node> presents = configuration.skillNodesThatRequireSource;
                        bool[] valid = new bool[presents.Count];
                        for (int x = 0; x < presents.Count; x++)
                        {
                            valid[x] = true;
                            Node present = presents[x];
                            if (!present.hasRequirements || node.requirements?.Count == 0)
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " is attempting present " + present.name + " but " + present.name + " does not require " + node.name);
                                valid[x] = false;
                                continue;
                            }
                            bool found = false;
                            foreach (I_NodeRequirements requirements in present.requirements)
                            {
                                if (requirements is OtherNodeRequirement otherRequirements && otherRequirements.node == node)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " is attempting present " + present.name + " but " + present.name + " does not require " + node.name);
                                valid[x] = false;
                                continue;
                            }
                        }
                        bool anyValid = false;
                        for (int x = 0; x < valid.Length; x++)
                        {
                            anyValid = anyValid || valid[x];
                        }
                        if (!anyValid)
                        {
                            continue;
                        }
                        Node focus = null;
                        if (presents.Count == 1)
                        {
                            focus = presents[0];
                        }
                        else
                        {
                            focus = configuration.reference;
                            if (!presents.Contains(focus))
                            {
                                Logger.ErrorLog("SkillNode: " + node.name + " is attempting to use skill node as focus that it is not presenting");
                                continue;
                            }
                        }

                        GameObject requirementGO = Instantiate(requirementsPrefab, requirementsHolder.transform);
                        requirements.Add(requirementGO);
                        NodeRequirementsPositionController controller = requirementGO.GetComponent<NodeRequirementsPositionController>();
                        controller.reference = transform;
                        List<NodeUI> nodeUIs = new List<NodeUI>();
                        List<I_NodeRequirements> baseRequirements = new List<I_NodeRequirements>();
                        OtherNodeRequirement focusRequirement = null;
                        foreach (I_NodeRequirements other in focus.requirements)
                        {
                            if (other is OtherNodeRequirement otherRequirements && otherRequirements.node == node)
                            {
                                baseRequirements.Add(other);
                                nodeUIs.Add(nodeConfigToElement[focus]);
                                focusRequirement = otherRequirements;
                            }
                        }
                        if (requirementControllers.TryGetValue(focus, out List<NodeRequirementsPositionController> controllers))
                        {
                            controllers.Add(controller);
                        }
                        else
                        {
                            requirementControllers.Add(focus, new List<NodeRequirementsPositionController>() { controller });
                        }
                        NodeUI focusUi = nodeConfigToElement[node];
                        controller.Initialize(baseRequirements, nodeUIs, focusUi, nodeConfigToElement[focus], focusRequirement.levelRequired);
                        controller.locationX = configuration.locationX;
                        controller.locationY = configuration.locationY;
                        controller.requiresBound = configuration.presentsBound;
                        controller.sourceBound = configuration.sourceBound;
                        controller.gameObject.name = node.name + " presenting " + count;
                        count++;
                    }
                }
            }
        }
    }
}
using Ashen.NodeTreeSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.SkillTreeSystem
{
    public class SkillTreeDefinition : SerializedScriptableObject
    {
        [Hide, BoxGroup("Base Skill Tree")]
        public NodeTreeEditor primarySkillTree;

        [OdinSerialize, HideInInspector]
        private List<SubSkillTreeDefinition> subSkillTrees;

        public List<SubSkillTreeDefinition> SubSkillTrees
        {
            get
            {
                if (subSkillTrees == null)
                {
                    subSkillTrees = new List<SubSkillTreeDefinition>();
                }
                return subSkillTrees;
            }
        }

        [TitleGroup("Sub Skill Trees")]
        [ValueToggleButton("@" + nameof(BuildValueToggle) + "()"), Hide]
        public int choice;

        public List<ValueDropdownItem<int>> BuildValueToggle()
        {
            List<ValueDropdownItem<int>> options = new();
            int x = 0;
            for (; x <= SubSkillTrees.Count; x++)
            {
                string name = BuildTitle(x);
                options.Add(new ValueDropdownItem<int>
                {
                    Value = x,
                    Text = name,
                });
            }
            if (options.Count == 1)
            {
                choice = -1;
            }
            return options;
        }

        private string BuildTitle()
        {
            return BuildTitle(choice);
        }

        private string BuildTitle(int index)
        {
            if (index >= SubSkillTrees.Count)
            {
                return "Add New Sub Skill Tree";
            }
            SubSkillTreeDefinition def = SubSkillTrees[index];
            if (def.name == null || string.IsNullOrEmpty(def.name.subclassName))
            {
                return "Unnamed Sub Skill";
            }
            return def.name.subclassName;
        }

        [BoxGroup("Sub Skill Trees/Skill Tree")]
        [OdinSerialize, Title("@" + nameof(BuildTitle) + "()"), Hide]
        public SubSkillTreeDefinition FocusedSubTree
        {
            get
            {
                if (choice == SubSkillTrees.Count)
                {
                    SubSkillTrees.Add(new SubSkillTreeDefinition());
                }
                if (choice >= 0 && choice < SubSkillTrees.Count)
                {
                    return SubSkillTrees[choice];
                }
                return null;
            }
            set
            {

            }
        }

        public List<Node> GetNodes(SubSkillTreeKey subSkillTreeKey)
        {
            List<Node> nodes = new();
            SubSkillTreeDefinition subSkillTree = null;
            if (subSkillTreeKey != null)
            {
                for (int x = 0; x < SubSkillTrees.Count; x++)
                {
                    if (subSkillTreeKey == SubSkillTrees[x].name)
                    {
                        subSkillTree = SubSkillTrees[x];
                        break;
                    }
                }
            }
            else
            {
                subSkillTree = new SubSkillTreeDefinition();
            }
            /*int maxColumns = Mathf.Max(primarySkillTree.Columns.Count, subSkillTree.Columns.Count);
            List<NodeUIContainer> emptyColumn = new();
            for (int x = 0; x < maxColumns; x++)
            {
                List<NodeUIContainer> primaryColumns = x < primarySkillTree.Columns.Count ? primarySkillTree.Columns[x] : emptyColumn;
                List<NodeUIContainer> subColumns = x < subSkillTree.Columns.Count ? subSkillTree.Columns[x] : emptyColumn;
                int maxRow = Mathf.Max(primaryColumns.Count, subColumns.Count);
                for (int y = 0; y < maxRow; y++)
                {
                    if (y < subColumns.Count && subColumns[y].Node)
                    {
                        nodes.Add(subColumns[y].Node);
                    }
                    else if (y < primaryColumns.Count && primaryColumns[y].Node)
                    {
                        nodes.Add(primaryColumns[y].Node);
                    }
                }
            }*/
            foreach (List<NodeUIContainer> containers in primarySkillTree.Columns)
            {
                foreach (NodeUIContainer container in containers)
                {
                    if (container.Node)
                    {
                        nodes.Add(container.Node);
                    }
                }
            }
            foreach (List<NodeUIContainer> containers in subSkillTree.Columns)
            {
                foreach (NodeUIContainer container in containers)
                {
                    if (container.Node)
                    {
                        nodes.Add(container.Node);
                    }
                }
            }
            return nodes;
        }

        public List<List<NodeUIContainer>> GetSkillTree(SubSkillTreeKey subSkillTreeKey)
        {
            List<List<NodeUIContainer>> columns = new();
            SubSkillTreeDefinition subSkillTree = null;
            if (subSkillTreeKey != null)
            {
                for (int x = 0; x < SubSkillTrees.Count; x++)
                {
                    if (subSkillTreeKey == SubSkillTrees[x].name)
                    {
                        subSkillTree = SubSkillTrees[x];
                        break;
                    }
                }
            }
            else
            {
                subSkillTree = new SubSkillTreeDefinition();
            }
            List<List<NodeUIContainer>> primaryColumns = primarySkillTree.Columns;
            List<List<NodeUIContainer>> secondaryColumns = subSkillTree.Columns;

            int primaryDepth = 0;
            if (primaryColumns.Count > 0)
            {
                foreach (NodeUIContainer container in primaryColumns[0])
                {
                    primaryDepth += container.Space;
                }
            }
            int secondaryDepth = 0;
            if (secondaryColumns.Count > 0)
            {
                foreach (NodeUIContainer container in secondaryColumns[0])
                {
                    secondaryDepth += container.Space;
                }
            }

            for (int x = 0; x < primaryColumns.Count || x < secondaryColumns.Count; x++)
            {
                List<NodeUIContainer> newColumn = new();
                columns.Add(newColumn);
                List<NodeUIContainer> primaryColumn = primaryColumns.Count > x ? primaryColumns[x] : null;
                List<NodeUIContainer> secondaryColumn = secondaryColumns.Count > x ? secondaryColumns[x] : null;

                if (primaryColumn != null)
                {
                    for (int y = 0; y < primaryColumn.Count; y++)
                    {
                        newColumn.Add(primaryColumn[y]);
                    }
                }
                else
                {
                    for (int y = 0; y < primaryDepth; y++)
                    {
                        newColumn.Add(new NodeUIContainer());
                    }
                }
                if (secondaryColumn != null)
                {
                    for (int y = 0; y < secondaryColumn.Count; y++)
                    {
                        newColumn.Add(secondaryColumn[y]);
                    }
                }
                else
                {
                    for (int y = 0; y < secondaryDepth; y++)
                    {
                        newColumn.Add(new NodeUIContainer());
                    }
                }
            }
            return columns;
        }
    }
}
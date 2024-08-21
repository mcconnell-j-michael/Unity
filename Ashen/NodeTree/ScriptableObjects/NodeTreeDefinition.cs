using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.NodeTreeSystem
{
    public class NodeTreeDefinition : SerializedScriptableObject
    {
        [Hide]
        public NodeTreeEditor nodeTree;

        public List<Node> Nodes
        {
            get
            {
                List<Node> nodes = new();
                foreach (List<NodeUIContainer> containers in Columns)
                {
                    foreach (NodeUIContainer container in containers)
                    {
                        if (container.node)
                        {
                            nodes.Add(container.node);
                        }
                    }
                }
                return nodes;
            }
        }
        public List<List<NodeUIContainer>> Columns
        {
            get
            {
                if (nodeTree == null)
                {
                    nodeTree = new NodeTreeEditor();
                }
                return nodeTree.Columns;
            }
        }

        //[HideInInspector]
        //[ValueToggleButton("@" + nameof(BuildValueToggle) + "()"), Hide]
        //public int choice;

        //public List<ValueDropdownItem<int>> BuildValueToggle()
        //{
        //    List<ValueDropdownItem<int>> options = new();
        //    int x = 0;
        //    for (; x < Columns.Count; x++)
        //    {
        //        options.Add(new ValueDropdownItem<int>
        //        {
        //            Value = x,
        //            Text = "Column " + (x + 1),
        //        });
        //    }
        //    if (options.Count == 0)
        //    {
        //        choice = -1;
        //    }
        //    options.Add(new ValueDropdownItem<int>
        //    {
        //        Value = x,
        //        Text = "Add Column",
        //    });
        //    return options;
        //}

        //private string BuildTitle()
        //{
        //    return "Column " + (choice + 1);
        //}

        //[HideInInspector]
        //[OdinSerialize, Title("@" + nameof(BuildTitle) + "()"), Hide, PropertyOrder(-1)]
        //public List<NodeUIContainer> FocusedColumn
        //{
        //    get
        //    {
        //        if (choice == Columns.Count)
        //        {
        //            columns.Add(new List<NodeUIContainer>());
        //        }
        //        if (choice >= 0 && choice < Columns.Count)
        //        {
        //            return Columns[choice];
        //        }
        //        return null;
        //    }
        //    set
        //    {

        //    }
        //}
    }

    public enum NodeTreeDirection
    {
        LeftToRight, BottomUp, TopDown
    }
}
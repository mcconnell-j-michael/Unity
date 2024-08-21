using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace Ashen.NodeTreeSystem
{
    [Serializable]
    public class NodeUIContainer
    {
        public NodeUIContainer()
        {
            type = NodeUiType.Empty;
            space = 1;
        }

        [HideLabel, EnumToggleButtons]
        public NodeUiType type;
        [PropertyRange(1, 7), ShowIf(nameof(type), Value = NodeUiType.SkillNode), OdinSerialize]
        private int space = 1;
        public int Space
        {
            get
            {
                if (type == NodeUiType.Empty)
                {
                    return 1;
                }
                if (space <= 0)
                {
                    return 1;
                }
                return space;
            }
            set
            {
                if (type == NodeUiType.Empty)
                {
                    return;
                }
                if (value < 1)
                {
                    return;
                }
                space = value;
            }
        }

        [ShowIf(nameof(type), Value = NodeUiType.SkillNode)]
        public Node node;

        public Node Node
        {
            get
            {
                if (type == NodeUiType.SkillNode)
                {
                    return node;
                }
                return null;
            }
        }

        public enum NodeUiType
        {
            Empty, SkillNode
        }
    }
}
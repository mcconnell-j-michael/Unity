using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.NodeTreeSystem
{
    public class NodeLibrary : SingletonScriptableObject<NodeLibrary>
    {
        public Dictionary<string, Node> idToNode;

#if UNITY_EDITOR
        [Button]
        public void LoadSkillNodes()
        {
            if (idToNode == null)
            {
                idToNode = new Dictionary<string, Node>();
            }
            List<Node> nodes = StaticUtilities.FindAssetsByType<Node>();
            foreach (Node node in nodes)
            {
                if (!idToNode.ContainsValue(node))
                {
                    if (node.displayName == null || node.displayName == "")
                    {
                        idToNode.Add(node.name, node);
                    }
                    else
                    {

                        idToNode.Add(node.displayName, node);
                    }
                }
            }
        }
#endif
    }
}
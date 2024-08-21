using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.NodeTreeSystem
{
    [Serializable]
    public class NodePresentsRequirementsConfiguration
    {
        public List<Node> skillNodesThatRequireSource;
        public Node reference;

        public RectTransformSide presentsBound;
        public RectTransformSide sourceBound;
        [Range(0, 100)]
        public int locationX;
        [Range(0, 100)]
        public int locationY;
    }
}
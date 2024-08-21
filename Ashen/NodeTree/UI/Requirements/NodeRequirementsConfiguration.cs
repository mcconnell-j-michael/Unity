using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.NodeTreeSystem
{
    [Serializable]
    public class NodeRequirementsConfiguration
    {
        public List<int> indexesOfRequirementsToBaseOn;
        [ShowIf("@" + nameof(indexesOfRequirementsToBaseOn) + ".Count > 1")]
        public int indexToBaseOn;

        public RectTransformSide requiresBound;
        public RectTransformSide sourceBound;
        [Range(0, 100)]
        public int locationX;
        [Range(0, 100)]
        public int locationY;
    }
}
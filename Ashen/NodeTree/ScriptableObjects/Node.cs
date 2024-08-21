using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.NodeTreeSystem
{
    public class Node : SerializedScriptableObject
    {
        public string displayName;

        [PropertyRange(1, nameof(GetMax))]
        public int maxRanks;
        private int GetMax
        {
            get
            {
                if (nodeElement)
                {
                    return nodeElement.levels;
                }
                return 1;
            }
        }

        public A_NodeElement nodeElement;

        public Texture skillImage;

        [TextArea]
        public string description;

        [ToggleGroup(nameof(hasRequirements))]
        public bool hasRequirements;

        [ToggleGroup(nameof(hasRequirements))]
        public List<I_NodeRequirements> requirements;

        [ToggleGroup(nameof(hasRequirements))]
        [Hide]
        public List<NodeRequirementsConfiguration> skilNodeRequirements;

        [ToggleGroup(nameof(presentsRequirements))]
        public bool presentsRequirements;

        [ToggleGroup(nameof(presentsRequirements))]
        [Hide]
        public List<NodePresentsRequirementsConfiguration> skillNodePresentsRequirements;
    }
}
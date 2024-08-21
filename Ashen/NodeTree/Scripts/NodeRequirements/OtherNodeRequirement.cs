using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.NodeTreeSystem
{
    public class OtherNodeRequirement : I_NodeRequirements
    {
        public Node node;
        //[ShowIf("@" + nameof(Max) + " > 1")]
        [PropertyRange(1, nameof(Max))]
        public int levelRequired = 1;

        [PropertyTooltip("If selected then 'enabled' lines will appear on top of disabled lines")]
        public bool enabledOnTop;
        [PropertyTooltip("If selected then treats the required skill node will be treated as the owner of the lines")]
        public bool inverseLineOwner;

        [AutoPopulate]
        public List<NodeLineConfiguration> lineConfigurations;

        public int Max
        {
            get
            {
                if (node)
                {
                    return node.maxRanks;
                }
                return 1;
            }
        }

        public bool RequirementsMet(I_NodeTreeManager nodeTreeManager)
        {
            if (!nodeTreeManager.HasSkillNode(node))
            {
                return true;
            }
            return nodeTreeManager.GetCurrentLevel(node) >= levelRequired;
        }
    }
}
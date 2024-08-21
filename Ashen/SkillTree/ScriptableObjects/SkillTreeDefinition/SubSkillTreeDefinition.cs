using Ashen.NodeTreeSystem;
using System.Collections.Generic;

namespace Ashen.SkillTreeSystem
{
    public class SubSkillTreeDefinition
    {
        public SubSkillTreeKey name;

        [Hide]
        public NodeTreeEditor primarySkillTree;

        public List<List<NodeUIContainer>> Columns
        {
            get
            {
                if (primarySkillTree == null)
                {
                    primarySkillTree = new NodeTreeEditor();
                }
                return primarySkillTree.Columns;
            }
        }
    }
}
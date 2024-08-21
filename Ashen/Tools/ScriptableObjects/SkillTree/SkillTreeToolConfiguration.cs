using Ashen.SkillTreeSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Manager
{
    public class SkillTreeToolConfiguration : A_Configuration<SkillTreeTool, SkillTreeToolConfiguration>
    {
        [SerializeField]
        private SkillTreeDefinition skillTree;
        [SerializeField, ToggleGroup(nameof(overrideSkillPoints))]
        private bool overrideSkillPoints;
        [SerializeField, ToggleGroup(nameof(overrideSkillPoints)), PropertyRange(0, 10)]
        private int defaultSkillPoints;

        public SkillTreeDefinition SkillTree
        {
            get
            {
                if (skillTree)
                {
                    return skillTree;
                }
                return GetDefault().skillTree;
            }
        }

        public int DefaultSkillPoints
        {
            get
            {
                if (IsDefault() || overrideSkillPoints)
                {
                    return defaultSkillPoints;
                }
                return GetDefault().defaultSkillPoints;
            }
        }
    }
}
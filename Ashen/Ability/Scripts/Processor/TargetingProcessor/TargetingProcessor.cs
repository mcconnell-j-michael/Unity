using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class TargetingProcessor : A_AbilityProcessor, I_BaseTargetingValue
    {
        private I_BaseTargetingValue targetingValue;

        public TargetingProcessor(I_BaseTargetingValue targetingValue)
        {
            this.targetingValue = targetingValue;
        }

        public List<AbilityTag> GetAbilityTags(ToolManager toolManager)
        {
            return targetingValue.GetAbilityTags(toolManager);
        }

        public List<I_TargetingRule> GetTargetingRules(ToolManager toolManager)
        {
            return targetingValue.GetTargetingRules(toolManager);
        }

        public TargetParty GetTargetParty()
        {
            return targetingValue.GetTargetParty();
        }

        public TargetRange GetTargetRange(ToolManager toolManager)
        {
            return targetingValue.GetTargetRange(toolManager);
        }

        public Target GetTargetType(ToolManager toolManager)
        {
            return targetingValue.GetTargetType(toolManager);
        }

        public I_BaseTargetingValue GetTargetingValue()
        {
            return targetingValue;
        }
    }
}

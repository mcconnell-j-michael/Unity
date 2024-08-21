using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class SubAbilityTargetingProcessor : I_BaseTargetingValue
    {
        public TargetingProcessor parentProcessor;

        public SubAbilityRelativeTarget relativeTarget;
        public TargetParty targetParty;

        public TargetParty GetTargetParty()
        {
            switch (relativeTarget)
            {
                case SubAbilityRelativeTarget.Self:
                    return TargetParty.ALLY;
                case SubAbilityRelativeTarget.Target:
                    return parentProcessor.GetTargetParty();
                case SubAbilityRelativeTarget.Random:
                    return targetParty;
            }
            return targetParty;
        }

        public Target GetTargetType(ToolManager toolManager)
        {
            return parentProcessor.GetTargetType(toolManager);
        }

        public TargetRange GetTargetRange(ToolManager toolManager)
        {
            return parentProcessor.GetTargetRange(toolManager);
        }

        public List<AbilityTag> GetAbilityTags(ToolManager toolManager)
        {
            List<AbilityTag> abilityTags = new List<AbilityTag>();
            abilityTags.AddRange(parentProcessor.GetAbilityTags(toolManager));
            return abilityTags;
        }

        public List<I_TargetingRule> GetTargetingRules(ToolManager toolManger)
        {
            List<I_TargetingRule> rules = new List<I_TargetingRule>();
            rules.AddRange(parentProcessor.GetTargetingRules(toolManger));
            return rules;
        }
    }
}
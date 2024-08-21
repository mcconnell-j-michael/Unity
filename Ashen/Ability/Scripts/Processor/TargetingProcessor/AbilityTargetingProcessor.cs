using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class AbilityTargetingProcessor : I_BaseTargetingValue
    {
        public TargetAttribute targetAttribute;
        public Target customTarget;
        public TargetRange customRange;
        public List<I_TargetingRule> customRules;
        public TargetParty targetParty;

        private bool useCustomAbilityTags;
        private List<AbilityTag> customAbilityTags;
        public List<AbilityTag> CustomAbilityTags
        {
            set
            {
                useCustomAbilityTags = true;
                customAbilityTags = value;
            }
        }

        public Target GetTargetType(ToolManager toolManager)
        {
            if (customTarget != null)
            {
                return customTarget;
            }
            ShiftableTargetTool targetTool = toolManager.Get<ShiftableTargetTool>();
            return targetTool.Get(targetAttribute);
        }

        public TargetRange GetTargetRange(ToolManager toolManager)
        {
            if (customRange)
            {
                return customRange;
            }
            ShiftableTargetRangeTool targetRangeTool = toolManager.Get<ShiftableTargetRangeTool>();
            return targetRangeTool.Get(targetAttribute);
        }

        public List<AbilityTag> GetAbilityTags(ToolManager toolManager)
        {
            List<AbilityTag> abilityTags = new();
            if (useCustomAbilityTags)
            {
                if (abilityTags != null)
                {
                    abilityTags.AddRange(customAbilityTags);
                }
                return abilityTags;
            }
            ShiftableAbilityTagTool abilityTagTool = toolManager.Get<ShiftableAbilityTagTool>();
            abilityTags.AddRange(abilityTagTool.Get(targetAttribute));
            return abilityTags;
        }

        public List<I_TargetingRule> GetTargetingRules(ToolManager toolManager)
        {
            List<I_TargetingRule> rules = new List<I_TargetingRule>();
            if (customRules != null)
            {
                rules.AddRange(customRules);
                return rules;
            }
            ShiftableTargetingRuleTool targetTool = toolManager.Get<ShiftableTargetingRuleTool>();
            rules.AddRange(targetTool.Get(targetAttribute));
            return rules;
        }

        public TargetParty GetTargetParty()
        {
            return targetParty;
        }
    }
}
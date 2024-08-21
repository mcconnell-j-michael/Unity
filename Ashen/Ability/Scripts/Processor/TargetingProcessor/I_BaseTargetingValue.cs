using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public interface I_BaseTargetingValue
    {
        Target GetTargetType(ToolManager toolManager);
        TargetRange GetTargetRange(ToolManager toolManager);
        List<AbilityTag> GetAbilityTags(ToolManager toolManager);
        List<I_TargetingRule> GetTargetingRules(ToolManager toolManager);
        TargetParty GetTargetParty();
    }
}

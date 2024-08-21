using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class RestrictIfHasExtendedEffectTag : I_TargetingRule
    {
        [EnumSODropdown, AutoPopulate]
        public List<ExtendedEffectTag> tags;

        public bool IsValidTarget(ToolManager source, ToolManager target, PartyPosition position)
        {
            if (!target)
            {
                return true;
            }
            StatusTool sTool = target.Get<StatusTool>();
            if (sTool)
            {
                foreach (ExtendedEffectTag tag in tags)
                {
                    if (sTool.CheckStatusEffectTag(tag))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class EnableIfHasExtendedEffectTag : I_TargetingRule
    {
        [EnumSODropdown, AutoPopulate]
        public List<ExtendedEffectTag> tags;

        public bool all;

        public bool IsValidTarget(ToolManager source, ToolManager target, PartyPosition position)
        {
            if (!target)
            {
                return true;
            }
            StatusTool sTool = target.Get<StatusTool>();
            bool anyFound = false;
            if (sTool)
            {
                foreach (ExtendedEffectTag tag in tags)
                {
                    if (sTool.CheckStatusEffectTag(tag))
                    {
                        anyFound = true;
                        if (!all)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (all)
                        {
                            return false;
                        }
                    }
                }
            }
            return anyFound;
        }
    }
}
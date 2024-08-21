using Ashen.AbilitySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableTargetingRuleTool : A_ShiftableCacheTool<ShiftableTargetingRuleTool, ShiftableTargetingRuleToolConfiguration, TargetAttribute, List<I_TargetingRule>, List<I_TargetingRule>, List<I_TargetingRule>>
    {
        public override A_Shiftable<List<I_TargetingRule>, List<I_TargetingRule>, List<I_TargetingRule>> GenerateShiftableValues()
        {
            int size = TargetAttributes.Count;
            ListShiftableAttribute<I_TargetingRule> shiftable = new(size);

            for (int x = 0; x < size; x++)
            {
                if (Config.GetDefaultValues().TryGetValue(TargetAttributes.Instance[x], out List<I_TargetingRule> tags))
                {
                    shiftable.SetDefault(x, new List<I_TargetingRule>(tags));
                }
                else
                {
                    shiftable.SetDefault(x, new List<I_TargetingRule>(Config.DefaultTargetingRule.GetRules()));
                }
            }

            return shiftable;
        }

        protected override IEnumerator<TargetAttribute> GetEnumeratorInternal()
        {
            return TargetAttributes.Instance.GetEnumerator();
        }

        protected override int GetEnumListSize()
        {
            return TargetAttributes.Count;
        }
    }
}
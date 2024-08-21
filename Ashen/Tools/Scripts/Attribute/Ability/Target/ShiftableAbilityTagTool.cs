using Ashen.AbilitySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableAbilityTagTool : A_ShiftableCacheTool<ShiftableAbilityTagTool, ShiftableAbilityTagToolConfiguration, TargetAttribute, List<AbilityTag>, List<AbilityTag>, List<AbilityTag>>
    {
        public override A_Shiftable<List<AbilityTag>, List<AbilityTag>, List<AbilityTag>> GenerateShiftableValues()
        {
            int size = TargetAttributes.Count;
            ListShiftableAttribute<AbilityTag> shiftable = new(size, true);

            for (int x = 0; x < size; x++)
            {
                if (Config.GetDefaultValues().TryGetValue(TargetAttributes.Instance[x], out List<AbilityTag> tags))
                {
                    shiftable.SetDefault(x, new List<AbilityTag>(tags));
                }
                else
                {
                    shiftable.SetDefault(x, new List<AbilityTag>(AbilityTags.Instance.defaultAbilityTags));
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
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableTargetRangeTool : A_ShiftableCacheTool<ShiftableTargetRangeTool, ShiftableTargetRangeToolConfiguration, TargetAttribute, TargetRange, TargetRange, TargetRange>
    {
        public override A_Shiftable<TargetRange, TargetRange, TargetRange> GenerateShiftableValues()
        {
            int size = TargetAttributes.Count;
            SingleShiftableAttribute<TargetRange> shiftable = new(size);

            for (int x = 0; x < size; x++)
            {
                if (Config.GetDefaultValues().TryGetValue(TargetAttributes.Instance[x], out TargetRange range))
                {
                    shiftable.SetDefault(x, range);
                }
                else
                {
                    shiftable.SetDefault(x, TargetRanges.Instance.defaultRange);
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
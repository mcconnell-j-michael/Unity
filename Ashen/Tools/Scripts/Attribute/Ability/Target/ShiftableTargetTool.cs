using Ashen.AbilitySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableTargetTool : A_ShiftableCacheTool<ShiftableTargetTool, ShiftableTargetToolConfiguration, TargetAttribute, Target, Target, Target>
    {
        public override A_Shiftable<Target, Target, Target> GenerateShiftableValues()
        {
            int size = TargetAttributes.Count;
            SingleShiftableAttribute<Target> shiftable = new(size);

            for (int x = 0; x < size; x++)
            {
                if (Config.GetDefaultValues().TryGetValue(TargetAttributes.Instance[x], out Target foundTarget))
                {
                    shiftable.SetDefault(x, foundTarget);
                }
                else
                {
                    shiftable.SetDefault(x, Targets.Instance.defaultTarget);
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
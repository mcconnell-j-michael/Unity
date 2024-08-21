using Ashen.AbilitySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class AbilityAttributeTool : A_ShiftableCacheTool<AbilityAttributeTool, AbilityAttributeToolConfiguration, AbilityTag, List<AbilityAttribute>, List<AbilityAttribute>, List<AbilityAttribute>>
    {
        public override A_Shiftable<List<AbilityAttribute>, List<AbilityAttribute>, List<AbilityAttribute>> GenerateShiftableValues()
        {
            int size = AbilityTags.Count;
            ListShiftableAttribute<AbilityAttribute> shiftable = new(size);

            for (int x = 0; x < size; x++)
            {
                if (Config.GetDefaultValues().TryGetValue(AbilityTags.Instance[x], out List<AbilityAttribute> tags))
                {
                    shiftable.SetDefault(x, new List<AbilityAttribute>(tags));
                }
                else
                {
                    shiftable.SetDefault(x, new List<AbilityAttribute>());
                }
            }

            return shiftable;
        }

        protected override IEnumerator<AbilityTag> GetEnumeratorInternal()
        {
            return AbilityTags.Instance.GetEnumerator();
        }

        protected override int GetEnumListSize()
        {
            return AbilityTags.Count;
        }
    }
}
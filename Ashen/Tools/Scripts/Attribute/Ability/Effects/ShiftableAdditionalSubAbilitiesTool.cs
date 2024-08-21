using Ashen.AbilitySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableAdditionalSubAbilitiesTool : A_ShiftableCacheTool<
        ShiftableAdditionalSubAbilitiesTool,
        ShiftableAdditionalSubAbilitiesToolConfiguration,
        TargetAttribute,
        List<SubAbilityBuilder>,
        List<SubAbilityBuilder>,
        List<SubAbilityBuilder>>
    {
        public override A_Shiftable<List<SubAbilityBuilder>, List<SubAbilityBuilder>, List<SubAbilityBuilder>> GenerateShiftableValues()
        {
            int size = TargetAttributes.Count;
            ListShiftableAttribute<SubAbilityBuilder> shiftable = new(size);

            for (int x = 0; x < size; x++)
            {
                if (Config.GetDefaultValues().TryGetValue(TargetAttributes.Instance[x], out List<SubAbilityBuilder> additionalSubAbilities))
                {
                    shiftable.SetDefault(x, new List<SubAbilityBuilder>(additionalSubAbilities));
                }
                else
                {
                    shiftable.SetDefault(x, new List<SubAbilityBuilder>());
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

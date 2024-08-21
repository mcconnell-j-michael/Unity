using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableAdditionalEffectsTool : A_ShiftableCacheTool<
        ShiftableAdditionalEffectsTool,
        ShiftableAdditionalEffectsToolConfiguration,
        TargetAttribute,
        List<I_EffectBuilder>,
        List<I_EffectBuilder>,
        List<I_EffectBuilder>>
    {
        public override A_Shiftable<List<I_EffectBuilder>, List<I_EffectBuilder>, List<I_EffectBuilder>> GenerateShiftableValues()
        {
            int size = TargetAttributes.Count;
            ListShiftableAttribute<I_EffectBuilder> shiftable = new(size);

            for (int x = 0; x < size; x++)
            {
                if (Config.GetDefaultValues().TryGetValue(TargetAttributes.Instance[x], out List<I_EffectBuilder> additionalEffects))
                {
                    shiftable.SetDefault(x, new List<I_EffectBuilder>(additionalEffects));
                }
                else
                {
                    shiftable.SetDefault(x, new List<I_EffectBuilder>());
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

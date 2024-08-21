using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class AbilityDeliveryPackProcessor : A_AbilityProcessor
    {
        public TargetAttribute targetAttribute;
        public DeliveryPackBuilder deliveryPack;

        [ShowInInspector, ReadOnly]
        private float?[] effectFloatArguments;

        public void SetEffectFloat(A_EffectFloatArgument argument, float value)
        {
            if (effectFloatArguments == null)
            {
                effectFloatArguments = new float?[EffectFloatArguments.Count];
            }
            effectFloatArguments[(int)argument] = value;
        }

        public void FillDeliveryArguments(DeliveryArgumentPacks deliveryArguments)
        {
            if (effectFloatArguments == null)
            {
                return;
            }
            EffectsArgumentPack effectsArguments = deliveryArguments.GetPack<EffectsArgumentPack>();
            foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
            {
                if (effectFloatArguments[(int)argument] != null)
                {
                    effectsArguments.SetFloatArgument(argument, (float)effectFloatArguments[(int)argument]);
                }
            }
        }

        public DeliveryPackBuilder GetDeliveryPack(ToolManager toolManager)
        {
            if (!toolManager || !targetAttribute)
            {
                return deliveryPack;
            }
            ShiftableAdditionalEffectsTool shiftableAdditionalEffectsTool = toolManager.Get<ShiftableAdditionalEffectsTool>();
            List<I_EffectBuilder> additionalEffects = shiftableAdditionalEffectsTool.Get(targetAttribute);
            ListEffectBuilder listEffectBuilder = new()
            {
                effects = new List<I_EffectBuilder>()
                {
                    deliveryPack.deliveryPack,
                }
            };
            listEffectBuilder.effects.AddRange(additionalEffects);
            return new DeliveryPackBuilder()
            {
                deliveryPack = listEffectBuilder,
                preFilters = deliveryPack.preFilters,
                postFilters = deliveryPack.postFilters
            };
        }

        public override void OnLoad(DeliveryArgumentPacks arguments)
        {
            FillDeliveryArguments(arguments);
        }
    }
}
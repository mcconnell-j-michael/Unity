using Ashen.CombatSystem;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class TieredProcessor : A_TargetProcessor
    {
        [OdinSerialize]
        private Dictionary<TargetCategory, float> categoryToRatio;

        private float[] categoryPercentage;
        private float[] CategoryPercentage
        {
            get
            {
                if (categoryPercentage == null)
                {
                    categoryPercentage = new float[TargetCategories.Count];
                    float currentRatio = 1f;
                    for (int x = 0; x < TargetCategories.Count; x++)
                    {
                        if (categoryToRatio != null && categoryToRatio.TryGetValue(TargetCategories.Instance[x], out float newRatio))
                        {
                            currentRatio = newRatio;
                        }
                        categoryPercentage[x] = currentRatio;
                    }
                }
                return categoryPercentage;
            }
        }

        public override I_CombatProcessor BuildProcessors(List<TargetResult> targets, AbilityAction action)
        {
            TargetingProcessor targetingProcessor = action.Get<TargetingProcessor>();
            AbilityDeliveryPackProcessor deliveryProcessor = action.Get<AbilityDeliveryPackProcessor>();
            AbilityHitChanceProcessor hitChanceProcessor = action.Get<AbilityHitChanceProcessor>();
            AbilityAnimationProcessor animationProcessor = action.Get<AbilityAnimationProcessor>();

            ListActionBundle actions = new ListActionBundle();
            foreach (TargetResult result in targets)
            {
                float percentage = CategoryPercentage[(int)result.category];
                actions.Bundles.Add(BuildProcessor(result,
                    percentage,
                    action,
                    targetingProcessor,
                    deliveryProcessor,
                    hitChanceProcessor,
                    animationProcessor)
                );
            }

            return actions;
        }
    }
}
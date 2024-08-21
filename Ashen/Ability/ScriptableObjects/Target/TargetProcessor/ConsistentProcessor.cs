using Ashen.CombatSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.AbilitySystem
{
    public class ConsistentProcessor : A_TargetProcessor
    {
        [SerializeField]
        private float damageRatio = 1f;

        public override I_CombatProcessor BuildProcessors(List<TargetResult> targets, AbilityAction action)
        {
            TargetingProcessor targetingProcessor = action.Get<TargetingProcessor>();
            AbilityDeliveryPackProcessor deliveryProcessor = action.Get<AbilityDeliveryPackProcessor>();
            AbilityHitChanceProcessor hitChanceProcessor = action.Get<AbilityHitChanceProcessor>();
            AbilityAnimationProcessor animationProcessor = action.Get<AbilityAnimationProcessor>();

            ListActionBundle actions = new ListActionBundle();
            foreach (TargetResult result in targets)
            {
                actions.Bundles.Add(BuildProcessor(result,
                    damageRatio,
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
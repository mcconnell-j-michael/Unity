using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public abstract class A_TargetProcessor : I_TargetProcessor
    {
        public abstract I_CombatProcessor BuildProcessors(List<TargetResult> targets, AbilityAction action);

        public I_CombatProcessor BuildProcessor(TargetResult result, float damageRatio, AbilityAction ability, TargetingProcessor targetingProcessor, AbilityDeliveryPackProcessor deliveryProcessor,
            AbilityHitChanceProcessor hitChanceProcessor, AbilityAnimationProcessor animationProcessor)
        {
            SubactionProcessor action = new SubactionProcessor
            {
                actionExecutable = new ActionExecutable(ability)
                {
                    builder = deliveryProcessor.GetDeliveryPack(null),
                    target = result.target,
                    source = result.source,
                    effectFloatArguments = GetDefaultEffectFloatArguments(result, targetingProcessor, damageRatio)
                }
            };
            if (animationProcessor.GetAnimation() != null)
            {
                AnimationCenterTracker tracker = result.target.Get<AnimationCenterTracker>();
                if (tracker)
                {
                    PartyPosition targetPosition = result.targetParty.GetPosition(result.target);
                    action.animationExecutable = new AnimationExecutable
                    {
                        animation = animationProcessor.GetAnimation(),
                        location = tracker.animationCenter.transform.position,
                        waitTime = 0.3f,
                        position = targetPosition,
                    };
                }
            }
            return action;
        }

        public float?[] GetDefaultEffectFloatArguments(TargetResult result, TargetingProcessor targetingProcessor, float damageRatio)
        {
            PartyPosition sourcePosition = result.sourceParty.GetPosition(result.source);
            PartyPosition targetPosition = result.targetParty.GetPosition(result.target);
            float?[] effectFloatArguments = new float?[EffectFloatArguments.Count];
            effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] = 1f;
            if (result.sourceParty != result.targetParty)
            {
                TargetRange range = targetingProcessor.GetTargetRange(result.source);
                float multiplier = range.GetMultiplierForRange(sourcePosition, targetPosition);
                effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] = multiplier;
            }
            effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] *= damageRatio;
            return effectFloatArguments;
        }
    }
}
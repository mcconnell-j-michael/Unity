using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public class RandomStrikesTargetHolder : A_TargetHolder<RandomStrikesTargetHolder>
    {
        [SerializeField]
        private int minHits;
        [SerializeField]
        private int maxHits;
        [SerializeField, Range(0, 100)]
        private int decayStart;
        [SerializeField, Range(0, 100)]
        private int decayRate;

        private int decay;
        private int targetCounter;
        private bool hasNext;

        protected override void Initialize()
        {
            decay = decayStart;
            targetCounter = 0;
            hasNext = true;
        }

        public override void GetRandomTargetable()
        { }

        public override void SetTargetable(ToolManager target)
        { }

        public override void InitializeTarget(I_Targetable targetable)
        {
            List<PartyPosition> validPositions = GetValidPositions();
            foreach (PartyPosition position in validPositions)
            {
                targetParty.GetTargetable(position).Selected();
            }
        }

        public override I_CombatProcessor ResolveTargetInternal()
        {
            List<PartyPosition> validPositions = GetValidPositions();
            PartyPosition position = GetRandomValidPosition();
            ToolManager manager = targetParty.GetToolManager(position);
            SubactionProcessor action = new SubactionProcessor
            {
                actionExecutable = new ActionExecutable(this.action)
                {
                    builder = deliveryProcessor.GetDeliveryPack(source),
                    target = manager,
                    source = source,
                    effectFloatArguments = GetDefaultEffectFloatArguments(position)
                }
            };
            if (animationProcessor.GetAnimation() != null)
            {
                AnimationCenterTracker tracker = manager.Get<AnimationCenterTracker>();
                action.animationExecutable = new AnimationExecutable
                {
                    animation = animationProcessor.GetAnimation(),
                    location = tracker.animationCenter.transform.position,
                    waitTime = 0.3f,
                    position = position,
                };
            }
            return action;
        }

        public override List<TargetResult> FinalizeResultsInternal()
        {
            List<PartyPosition> validPositions = GetValidPositions();
            List<TargetResult> results = new();

            PartyPosition position = GetRandomValidPosition();
            ToolManager manager = targetParty.GetToolManager(position);
            targetCounter++;

            CalculateHasNext();

            results.Add(new TargetResult()
            {
                category = TargetCategories.Instance.DEFAULT_CATEGORY,
                source = source,
                target = manager,
                sourceParty = sourceParty,
                targetParty = targetParty
            });
            return results;
        }

        private void CalculateHasNext()
        {
            if (targetCounter < minHits)
            {
                return;
            }
            if (targetCounter >= maxHits)
            {
                hasNext = false;
                return;
            }
            if (Random.Range(0, 100) < decay)
            {
                decay -= decayRate;
            }
            else
            {
                hasNext = false;
                return;
            }
        }

        public override I_TargetHolder Clone()
        {
            return new RandomStrikesTargetHolder
            {
                decayRate = decayRate,
                decayStart = decayStart,
                maxHits = maxHits,
                minHits = minHits
            };
        }

        public override bool HasNextTargetInternal()
        {
            return hasNext;
        }

        public override void GetTargetableByThreat()
        { }

        public override I_Targetable RequestMove(MoveDirection moveDirection)
        {
            return targetParty.GetTargetable(GetValidPositions()[0]);
        }
    }
}
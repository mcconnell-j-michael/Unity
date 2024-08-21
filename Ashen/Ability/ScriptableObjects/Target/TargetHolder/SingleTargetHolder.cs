using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public class SingleTargetHolder : A_TargetHolder<SingleTargetHolder>
    {
        private I_Targetable target;

        private bool resolvedTarget;

        public override void GetRandomTargetable()
        {
            List<PartyPosition> positions = GetValidPositions();

            PartyPosition pos = GetRandomOrderedRowValidPosition();
            if (pos != null)
            {
                target = targetParty.GetTargetable(pos);
            }
        }

        public override void InitializeTarget(I_Targetable targetable)
        {
            if (target != null)
            {
                target.Deselected();
            }
            targetable.Selected();
            target = targetable;
        }

        public override void SetTargetable(ToolManager target)
        {
            PartyPosition position = targetParty.GetPosition(target);
            this.target = targetParty.GetTargetable(position);
        }

        protected override void Initialize()
        {
            resolvedTarget = false;
        }

        public override bool HasNextTargetInternal()
        {
            return !resolvedTarget;
        }

        public override I_CombatProcessor ResolveTargetInternal()
        {
            ToolManager primaryManager = target.GetTarget();
            PartyPosition primaryPosition = targetParty.GetPosition(primaryManager);

            List<PartyPosition> validPositions = GetValidPositions();

            if (validPositions.Count == 0)
            {
                return null;
            }

            if (!validPositions.Contains(primaryPosition))
            {
                primaryPosition = null;
            }

            if (primaryPosition == null)
            {
                primaryManager = null;
                primaryPosition = GetRandomOrderedRowValidPosition();
                primaryManager = targetParty.GetToolManager(primaryPosition);
            }

            SubactionProcessor action = new SubactionProcessor
            {
                actionExecutable = new ActionExecutable(this.action)
                {
                    builder = deliveryProcessor.GetDeliveryPack(source),
                    target = primaryManager,
                    source = source,
                    effectFloatArguments = GetDefaultEffectFloatArguments(primaryPosition)
                }
            };
            if (animationProcessor.GetAnimation() != null)
            {
                AnimationCenterTracker tracker = primaryManager.Get<AnimationCenterTracker>();
                if (tracker)
                {
                    action.animationExecutable = new AnimationExecutable
                    {
                        animation = animationProcessor.GetAnimation(),
                        location = tracker.animationCenter.transform.position,
                        waitTime = 0.3f,
                        position = primaryPosition,
                    };
                }
            }

            resolvedTarget = true;

            return action;
        }

        public override List<TargetResult> FinalizeResultsInternal()
        {
            resolvedTarget = true;
            List<TargetResult> results = new();

            ToolManager primaryManager = target.GetTarget();
            PartyPosition primaryPosition = targetParty.GetPosition(primaryManager);

            List<PartyPosition> validPositions = GetValidPositions();

            if (validPositions.Count == 0)
            {
                return null;
            }

            if (!validPositions.Contains(primaryPosition))
            {
                primaryPosition = null;
            }

            if (primaryPosition == null)
            {
                primaryManager = null;
                primaryPosition = GetRandomOrderedRowValidPosition();
                primaryManager = targetParty.GetToolManager(primaryPosition);
            }

            results.Add(new TargetResult()
            {
                category = TargetCategories.Instance.DEFAULT_CATEGORY,
                source = source,
                target = primaryManager,
                sourceParty = sourceParty,
                targetParty = targetParty
            });

            return results;
        }

        public override I_Targetable RequestMove(MoveDirection moveDirection)
        {
            PartyPosition currentPosition = targetParty.GetPosition(target.GetTarget());
            PartyPosition next = GetValidPosition(currentPosition, moveDirection);

            if (currentPosition == next)
            {
                return target;
            }

            target.Deselected();
            target = targetParty.GetTargetable(next);
            target.Selected();

            return target;
        }

        public override void GetTargetableByThreat()
        {
            PartyPosition position = this.GetPositionByThreat();
            target = targetParty.GetTargetable(position);
        }
    }
}
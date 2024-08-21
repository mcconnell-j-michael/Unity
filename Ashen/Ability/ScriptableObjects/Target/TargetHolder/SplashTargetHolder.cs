using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public class SplashTargetHolder : A_TargetHolder<SplashTargetHolder>
    {
        public TargetCategory splashCategory;
        public float splashDamageRatio = 1f;

        private I_Targetable target;

        private bool resolvedTarget;

        protected override void Initialize()
        {
            resolvedTarget = false;
        }

        public override I_TargetHolder Clone()
        {
            return new SplashTargetHolder
            {
                splashDamageRatio = this.splashDamageRatio
            };
        }

        public override void GetRandomTargetable()
        {
            List<PartyPosition> positions = GetValidPositions();

            PartyPosition pos = GetRandomOrderedRowValidPosition();
            if (pos != null)
            {
                target = targetParty.GetTargetable(pos);
            }
        }

        public override void SetTargetable(ToolManager target)
        {
            PartyPosition position = targetParty.GetPosition(target);
            this.target = targetParty.GetTargetable(position);
        }

        public override void InitializeTarget(I_Targetable targetable)
        {
            PartyPosition position = targetParty.GetPosition(targetable.GetTarget());

            target = targetParty.GetTargetable(position);
            target.Selected();

            PartyPosition leftPosition = targetParty.GetPreviousTargetableCharacterInRow(position);

            if (leftPosition != null)
            {
                I_Targetable leftTarget = targetParty.GetTargetable(leftPosition);
                leftTarget.SelectedSecondary();
            }

            PartyPosition rightPosition = targetParty.GetNextTargetableCharacterInRow(position);

            if (rightPosition != null)
            {
                I_Targetable rightTarget = targetParty.GetTargetable(rightPosition);
                rightTarget.SelectedSecondary();
            }
        }

        public override bool HasNextTargetInternal()
        {
            return !resolvedTarget;
        }

        public override I_CombatProcessor ResolveTargetInternal()
        {
            resolvedTarget = true;
            ListActionBundle actions = new ListActionBundle();

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
                if (primaryPosition == null)
                {
                    primaryManager = null;
                    primaryPosition = GetRandomOrderedRowValidPosition();
                    primaryManager = targetParty.GetToolManager(primaryPosition);
                }
            }

            actions.Bundles.Add(new SubactionProcessor()
            {
                actionExecutable = new ActionExecutable(action)
                {
                    builder = deliveryProcessor.GetDeliveryPack(source),
                    source = source,
                    target = primaryManager,
                    effectFloatArguments = GetDefaultEffectFloatArguments(primaryPosition)
                }
            });

            PartyPosition leftPosition = targetParty.GetPreviousTargetableCharacterInRow(primaryPosition);
            if (leftPosition != null)
            {
                float?[] effectFloatArguments = GetDefaultEffectFloatArguments(leftPosition);
                effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] *= this.splashDamageRatio;
                ToolManager leftManager = targetParty.GetToolManager(leftPosition);
                actions.Bundles.Add(new SubactionProcessor()
                {
                    actionExecutable = new ActionExecutable(action)
                    {
                        builder = deliveryProcessor.GetDeliveryPack(source),
                        source = source,
                        target = leftManager,
                        effectFloatArguments = effectFloatArguments,
                    }
                });
            }

            PartyPosition rightPosition = targetParty.GetNextTargetableCharacterInRow(primaryPosition);

            if (rightPosition != null)
            {
                float?[] effectFloatArguments = GetDefaultEffectFloatArguments(rightPosition);
                effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] *= this.splashDamageRatio;
                ToolManager rightManager = targetParty.GetToolManager(rightPosition);
                actions.Bundles.Add(new SubactionProcessor()
                {
                    actionExecutable = new ActionExecutable(action)
                    {
                        builder = deliveryProcessor.GetDeliveryPack(source),
                        source = source,
                        target = rightManager,
                        effectFloatArguments = effectFloatArguments,
                    }
                });
            }

            if (animationProcessor.GetAnimation() != null)
            {
                AnimationCenterTracker tracker = primaryManager.Get<AnimationCenterTracker>();
                actions.Bundles.Add(new StandaloneAnimationBundle()
                {
                    animationExecutable = new AnimationExecutable()
                    {
                        animation = animationProcessor.GetAnimation(),
                        location = tracker.animationCenter.transform.position,
                        waitTime = 0.3f,
                        position = primaryPosition,
                    }
                });
            }

            return actions;
        }

        public override List<TargetResult> FinalizeResultsInternal()
        {
            resolvedTarget = true;

            List<TargetResult> results = new();
            resolvedTarget = true;

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
                if (primaryPosition == null)
                {
                    primaryPosition = GetRandomOrderedRowValidPosition();
                    primaryManager = targetParty.GetToolManager(primaryPosition);
                }
            }

            results.Add(new TargetResult()
            {
                category = TargetCategories.Instance.DEFAULT_CATEGORY,
                source = source,
                target = primaryManager,
                sourceParty = sourceParty,
                targetParty = targetParty
            });

            PartyPosition leftPosition = targetParty.GetPreviousTargetableCharacterInRow(primaryPosition);
            if (leftPosition != null)
            {
                ToolManager leftManager = targetParty.GetToolManager(leftPosition);
                results.Add(new TargetResult()
                {
                    category = splashCategory,
                    source = source,
                    target = leftManager,
                    sourceParty = sourceParty,
                    targetParty = targetParty
                });
            }

            PartyPosition rightPosition = targetParty.GetNextTargetableCharacterInRow(primaryPosition);

            if (rightPosition != null)
            {
                ToolManager rightManager = targetParty.GetToolManager(rightPosition);
                results.Add(new TargetResult()
                {
                    category = splashCategory,
                    source = source,
                    target = rightManager,
                    sourceParty = sourceParty,
                    targetParty = targetParty
                });
            }

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

            targetParty.DeselectAll();

            target = targetParty.GetTargetable(next);
            target.Selected();

            PartyPosition leftPosition = targetParty.GetPreviousTargetableCharacterInRow(next);

            if (leftPosition != null)
            {
                I_Targetable leftTarget = targetParty.GetTargetable(leftPosition);
                leftTarget.SelectedSecondary();
            }

            PartyPosition rightPosition = targetParty.GetNextTargetableCharacterInRow(next);

            if (rightPosition != null)
            {
                I_Targetable rightTarget = targetParty.GetTargetable(rightPosition);
                rightTarget.SelectedSecondary();
            }

            return target;
        }

        public override void GetTargetableByThreat()
        {
            PartyPosition position = this.GetPositionByThreat();
            target = targetParty.GetTargetable(position);
        }
    }
}
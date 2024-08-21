using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public class PiercingTargetHolder : A_TargetHolder<PiercingTargetHolder>
    {
        public float backDamageRatio;
        public TargetCategory backTargetCategory;

        private I_Targetable primary;
        private I_Targetable backTarget;

        private bool resolvedTarget;

        protected override void Initialize()
        {
            resolvedTarget = false;
        }

        public override I_TargetHolder Clone()
        {
            return new PiercingTargetHolder
            {
                backDamageRatio = backDamageRatio
            };
        }

        public override void GetRandomTargetable()
        {
            ToolManager primaryManager = null;
            PartyPosition primaryPosition = null;

            resolvedTarget = true;

            List<PartyPosition> validPositions = GetValidPositions();

            primaryPosition = GetRandomOrderedRowValidPosition();
            if (primaryPosition == null)
            {
                return;
            }

            primaryManager = targetParty.GetToolManager(primaryPosition);

            primary = targetParty.GetTargetable(primaryPosition);

            if (primaryPosition.partyRow.backward == PartyRows.Instance.First())
            {
                return;
            }

            PartyPosition backPosition = GetRandomRowValidPosition(primaryPosition.partyRow.backward);
            if (backPosition != null)
            {
                ToolManager backManager = targetParty.GetToolManager(backPosition);
                backTarget = targetParty.GetTargetable(backPosition);
            }
        }

        public override void InitializeTarget(I_Targetable targetable)
        {
            primary = null;
            backTarget = null;
            PartyPosition position = targetParty.GetPosition(targetable.GetTarget());
            primary = targetParty.GetTargetable(position);

            List<PartyPosition> validPositions = GetValidPositions();

            primary = targetParty.GetTargetable(position);
            primary.Selected();

            if (position.partyRow.backward != PartyRows.Instance.First())
            {
                foreach (PartyPosition pos in validPositions)
                {
                    if (pos.partyRow == position.partyRow.backward)
                    {
                        backTarget = targetParty.GetTargetable(pos);
                        break;
                    }
                }

                if (backTarget != null)
                {
                    backTarget.SelectedSecondary();
                }
            }
        }

        public override void SetTargetable(ToolManager target)
        {
            PartyPosition position = targetParty.GetPosition(target);
            primary = targetParty.GetTargetable(position);
            InitializeTarget(primary);
        }

        public override I_CombatProcessor ResolveTargetInternal()
        {
            resolvedTarget = true;

            List<PartyPosition> validPositions = GetValidPositions();

            ToolManager primaryManager = primary.GetTarget();
            PartyPosition primaryPosition = targetParty.GetPosition(primaryManager);

            if (validPositions.Count == 0)
            {
                return null;
            }

            if (!validPositions.Contains(primaryPosition))
            {
                primaryPosition = null;
            }

            ListActionBundle actions = new ListActionBundle();

            if (primaryPosition == null)
            {
                GetRandomTargetable();
                primaryManager = primary.GetTarget();
                primaryPosition = targetParty.GetPosition(primaryManager);
            }

            actions.Bundles.Add(new SubactionProcessor()
            {
                actionExecutable = new ActionExecutable(action)
                {
                    builder = deliveryProcessor.GetDeliveryPack(source),
                    target = primaryManager,
                    source = source,
                    effectFloatArguments = GetDefaultEffectFloatArguments(primaryPosition)
                }
            });

            if (primaryPosition.partyRow.backward == PartyRows.Instance.First())
            {
                return actions;
            }

            ToolManager backManager = null;
            PartyPosition backPosition = null;

            if (backTarget != null)
            {
                backManager = backTarget.GetTarget();
                backPosition = targetParty.GetPosition(backManager);
            }

            if (backManager == null || backPosition == null || backPosition.partyRow != primaryPosition.partyRow.backward)
            {
                backManager = null;
                backPosition = null;
                backPosition = GetRandomRowValidPosition(primaryPosition.partyRow.backward);
                if (backPosition)
                {
                    backManager = targetParty.GetToolManager(backPosition);
                }
                else
                {
                    return actions;
                }
            }
            float?[] effectFloatArguments = GetDefaultEffectFloatArguments(backPosition);
            effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] *= this.backDamageRatio;
            actions.Bundles.Add(new SubactionProcessor()
            {
                actionExecutable = new ActionExecutable(action)
                {
                    builder = deliveryProcessor.GetDeliveryPack(source),
                    target = backManager,
                    source = source,
                    effectFloatArguments = effectFloatArguments,
                }
            });

            return actions;
        }

        public override List<TargetResult> FinalizeResultsInternal()
        {
            resolvedTarget = true;

            List<TargetResult> results = new();

            List<PartyPosition> validPositions = GetValidPositions();

            ToolManager primaryManager = primary.GetTarget();
            PartyPosition primaryPosition = targetParty.GetPosition(primaryManager);

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
                GetRandomTargetable();
                primaryManager = primary.GetTarget();
                primaryPosition = targetParty.GetPosition(primaryManager);
            }

            results.Add(new TargetResult()
            {
                category = TargetCategories.Instance.DEFAULT_CATEGORY,
                source = source,
                target = primaryManager,
                sourceParty = sourceParty,
                targetParty = targetParty
            });

            if (primaryPosition.partyRow.backward == PartyRows.Instance.First())
            {
                return results;
            }

            ToolManager backManager = null;
            PartyPosition backPosition = null;

            if (backTarget != null)
            {
                backManager = backTarget.GetTarget();
                backPosition = targetParty.GetPosition(backManager);
            }

            if (backManager == null || backPosition == null || backPosition.partyRow != primaryPosition.partyRow.backward)
            {
                backPosition = GetRandomRowValidPosition(primaryPosition.partyRow.backward);
                if (backPosition)
                {
                    backManager = targetParty.GetToolManager(backPosition);
                }
                else
                {
                    return results;
                }
            }

            results.Add(new TargetResult()
            {
                category = backTargetCategory,
                source = source,
                target = backManager,
                sourceParty = sourceParty,
                targetParty = targetParty
            });

            return results;
        }

        public override I_Targetable RequestMove(MoveDirection moveDirection)
        {
            PartyPosition currentPosition = targetParty.GetPosition(primary.GetTarget());
            PartyPosition nextPosition = GetValidPosition(currentPosition, moveDirection);

            if (currentPosition.partyRow != nextPosition.partyRow)
            {
                primary.Deselected();
                if (backTarget != null)
                {
                    backTarget.Deselected();
                }
                InitializeTarget(targetParty.GetTargetable(nextPosition));
                return primary;
            }
            if (moveDirection == MoveDirection.Down || moveDirection == MoveDirection.Up)
            {
                return primary;
            }
            if (backTarget == null)
            {
                if (nextPosition == currentPosition)
                {
                    return primary;
                }
                primary.Deselected();
                primary = targetParty.GetTargetable(nextPosition);
                primary.Selected();
                return primary;
            }
            PartyPosition curBack = targetParty.GetPosition(backTarget.GetTarget());
            PartyPosition nextBack = GetValidPosition(curBack, moveDirection);

            if ((moveDirection == MoveDirection.Right && nextBack.Index > curBack.Index) ||
                (moveDirection == MoveDirection.Left && nextBack.Index < curBack.Index))
            {
                backTarget.Deselected();
                backTarget = targetParty.GetTargetable(nextBack);
                backTarget.SelectedSecondary();
                return primary;
            }

            if (currentPosition == nextPosition)
            {
                if (curBack != nextBack)
                {
                    backTarget.Deselected();
                    backTarget = targetParty.GetTargetable(nextBack);
                    backTarget.SelectedSecondary();
                }
                return primary;
            }

            primary.Deselected();
            primary = targetParty.GetTargetable(nextPosition);
            primary.Selected();
            if (curBack != nextBack)
            {
                backTarget.Deselected();
                backTarget = targetParty.GetTargetable(nextBack);
                backTarget.SelectedSecondary();
            }

            return primary;
        }

        public override bool HasNextTargetInternal()
        {
            return !resolvedTarget;
        }

        public override void GetTargetableByThreat()
        {
            PartyPosition position = this.GetPositionByThreat();
            primary = targetParty.GetTargetable(position);
        }
    }
}
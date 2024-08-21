using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public class RowTargetHolder : A_TargetHolder<RowTargetHolder>
    {
        private PartyRow currentRow;

        private bool resolvedTarget;

        protected override void Initialize()
        {
            resolvedTarget = false;
        }

        public override void GetRandomTargetable()
        {
            List<PartyPosition> validPositions = GetValidPositions();
            if (validPositions.Count == 0)
            {
                return;
            }
            int random = Random.Range(0, validPositions.Count);
            currentRow = validPositions[random].partyRow;
        }

        public override void InitializeTarget(I_Targetable targetable)
        {
            PartyPosition position = targetParty.GetPosition(targetable.GetTarget());
            currentRow = position.partyRow;
            foreach (PartyPosition nextPosition in GetValidPositions())
            {
                if (nextPosition.partyRow != currentRow)
                {
                    continue;
                }
                I_Targetable newTargetable = targetParty.GetTargetable(nextPosition);
                newTargetable.Selected();
            }
        }

        public override void SetTargetable(ToolManager target)
        {
            PartyPosition position = targetParty.GetPosition(target);
            currentRow = position.partyRow;
        }

        public override I_CombatProcessor ResolveTargetInternal()
        {
            resolvedTarget = true;
            ListActionBundle actions = new ListActionBundle();
            List<PartyPosition> validPositions = GetValidPositions();

            if (validPositions.Count == 0)
            {
                return null;
            }

            bool validRow = false;

            foreach (PartyPosition position in validPositions)
            {
                if (position.partyRow == currentRow)
                {
                    validRow = true;
                }
            }

            if (!validRow)
            {
                currentRow = null;
                PartyRow first = PartyRows.Instance.First();
                PartyRow row = first;

                do
                {
                    foreach (PartyPosition position in validPositions)
                    {
                        if (position.partyRow == row)
                        {
                            currentRow = row;
                        }
                    }
                    row = row.backward;
                } while (row != first && currentRow == null);
            }

            if (currentRow == null)
            {
                return null;
            }

            foreach (PartyPosition position in GetValidPositionsInRow(currentRow))
            {
                if (!validPositions.Contains(position))
                {
                    continue;
                }
                ToolManager manager = targetParty.GetToolManager(position);
                actions.Bundles.Add(new SubactionProcessor()
                {
                    actionExecutable = new ActionExecutable(action)
                    {
                        builder = deliveryProcessor.GetDeliveryPack(source),
                        target = manager,
                        source = source,
                        effectFloatArguments = GetDefaultEffectFloatArguments(position)
                    }
                });
            }
            return actions;
        }

        public override List<TargetResult> FinalizeResultsInternal()
        {
            List<PartyPosition> validPositions = GetValidPositions();
            List<TargetResult> results = new();

            resolvedTarget = true;

            if (validPositions.Count == 0)
            {
                return null;
            }

            bool validRow = false;

            foreach (PartyPosition position in validPositions)
            {
                if (position.partyRow == currentRow)
                {
                    validRow = true;
                }
            }

            if (!validRow)
            {
                currentRow = null;
                PartyRow first = PartyRows.Instance.First();
                PartyRow row = first;

                do
                {
                    foreach (PartyPosition position in validPositions)
                    {
                        if (position.partyRow == row)
                        {
                            currentRow = row;
                        }
                    }
                    row = row.backward;
                } while (row != first && currentRow == null);
            }

            if (currentRow == null)
            {
                return null;
            }

            foreach (PartyPosition position in GetValidPositionsInRow(currentRow))
            {
                if (!validPositions.Contains(position))
                {
                    continue;
                }
                ToolManager manager = targetParty.GetToolManager(position);
                results.Add(new TargetResult()
                {
                    category = TargetCategories.Instance.DEFAULT_CATEGORY,
                    source = source,
                    target = manager,
                    sourceParty = sourceParty,
                    targetParty = targetParty
                });
            }
            return results;
        }

        private void SelectAllForRow(PartyPosition position, List<PartyPosition> validPostions)
        {
            foreach (PartyPosition prevPosition in PartyPositions.Instance)
            {
                I_Targetable targetable = targetParty.GetTargetable(prevPosition);
                if (targetable != null)
                {
                    targetable.Deselected();
                }
            }

            currentRow = position.partyRow;

            foreach (PartyPosition nextPosition in GetValidPositionsInRow(currentRow))
            {
                if (validPostions.Contains(nextPosition))
                {
                    I_Targetable targetable = targetParty.GetTargetable(nextPosition);
                    if (targetable != null)
                    {
                        targetable.Selected();
                    }
                }
            }
        }

        public override I_Targetable RequestMove(MoveDirection moveDirection)
        {
            List<PartyPosition> validPositions = GetValidPositions();
            if (moveDirection == MoveDirection.Left || moveDirection == MoveDirection.Right || moveDirection == MoveDirection.None)
            {
                foreach (PartyPosition position in validPositions)
                {
                    if (position.partyRow == currentRow)
                    {
                        return targetParty.GetTargetable(position);
                    }
                }
            }

            bool valid = false;
            foreach (PartyPosition position in GetValidPositions())
            {
                if (position.partyRow != currentRow)
                {
                    valid = true;
                }
            }

            if (!valid)
            {
                foreach (PartyPosition position in validPositions)
                {
                    if (position.partyRow == currentRow)
                    {
                        return targetParty.GetTargetable(position);
                    }
                }
            }

            targetParty.DeselectAll();

            if (moveDirection == MoveDirection.Up)
            {
                if (targetParty == sourceParty)
                {
                    currentRow = currentRow.forward;
                }
                else
                {
                    currentRow = currentRow.backward;
                }
            }
            else if (moveDirection == MoveDirection.Down)
            {
                if (targetParty == sourceParty)
                {
                    currentRow = currentRow.backward;
                }
                else
                {
                    currentRow = currentRow.forward;
                }
            }

            foreach (PartyPosition nextPosition in GetValidPositions())
            {
                if (nextPosition.partyRow != currentRow)
                {
                    continue;
                }
                I_Targetable targetable = targetParty.GetTargetable(nextPosition);
                targetable.Selected();
            }

            foreach (PartyPosition position in validPositions)
            {
                if (position.partyRow == currentRow)
                {
                    return targetParty.GetTargetable(position);
                }
            }

            return null;
        }

        public override bool HasNextTargetInternal()
        {
            return !resolvedTarget;
        }

        public override void GetTargetableByThreat()
        {
            PartyPosition position = this.GetPositionByThreat();
            currentRow = position.partyRow;
        }
    }
}
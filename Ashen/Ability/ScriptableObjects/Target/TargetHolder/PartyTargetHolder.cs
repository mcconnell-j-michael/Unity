using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public class PartyTargetHolder : A_TargetHolder<PartyTargetHolder>
    {
        private bool resolvedTarget;

        public override void GetRandomTargetable()
        { }

        public override void GetTargetableByThreat()
        { }

        public override bool HasNextTargetInternal()
        {
            return !resolvedTarget;
        }

        protected override void Initialize()
        {
            base.Initialize();
            resolvedTarget = false;
        }

        public override I_CombatProcessor ResolveTargetInternal()
        {
            resolvedTarget = true;
            ListActionBundle actions = new ListActionBundle();
            List<PartyPosition> validPositions = GetValidPositions();
            foreach (PartyPosition position in targetParty.GetActivePositions())
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
                        source = source,
                        target = manager,
                        effectFloatArguments = GetDefaultEffectFloatArguments(position)
                    }
                });
            }
            return actions;
        }

        public override List<TargetResult> FinalizeResultsInternal()
        {
            resolvedTarget = true;
            List<PartyPosition> validPositions = GetValidPositions();
            List<TargetResult> results = new();
            foreach (PartyPosition position in targetParty.GetActivePositions())
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

        public override void InitializeTarget(I_Targetable targetable)
        {
            List<PartyPosition> validPositions = GetValidPositions();
            foreach (PartyPosition position in validPositions)
            {
                targetParty.GetTargetable(position).Selected();
            }
        }

        public override void SetTargetable(ToolManager target)
        { }

        public override I_Targetable RequestMove(MoveDirection moveDirection)
        {
            return targetParty.GetTargetable(GetValidPositions()[0]);
        }
    }
}
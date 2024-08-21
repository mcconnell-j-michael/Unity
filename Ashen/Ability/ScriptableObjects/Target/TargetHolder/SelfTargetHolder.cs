using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public class SelfTargetHolder : A_TargetHolder<SelfTargetHolder>
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

        public override void InitializeTarget(I_Targetable targetable)
        {
            targetable.Selected();
        }

        public override void SetTargetable(ToolManager target)
        { }

        protected override void Initialize()
        {
            resolvedTarget = false;
        }

        public override I_CombatProcessor ResolveTargetInternal()
        {
            resolvedTarget = true;
            ListActionBundle actions = new ListActionBundle();
            actions.Bundles.Add(new SubactionProcessor()
            {
                actionExecutable = new ActionExecutable(action)
                {
                    builder = deliveryProcessor.GetDeliveryPack(source),
                    source = source,
                    target = source,
                    effectFloatArguments = GetDefaultEffectFloatArguments(sourceParty.GetPosition(source))
                }
            });
            return actions;
        }

        public override List<TargetResult> FinalizeResultsInternal()
        {
            resolvedTarget = true;
            List<TargetResult> results = new()
            {
                new TargetResult()
                {
                    category = TargetCategories.Instance.DEFAULT_CATEGORY,
                    source = source,
                    target = source,
                    sourceParty = sourceParty,
                    targetParty = targetParty
                }
            };
            return results;
        }

        public override I_Targetable RequestMove(MoveDirection moveDirection)
        {
            return targetParty.GetTargetable(targetParty.GetPosition(source));
        }

        protected override void TargetHolderRestrictions(List<PartyPosition> currentValidPositions)
        {
            PartyPosition selfPosition = sourceParty.GetPosition(source);
            for (int x = 0; x < currentValidPositions.Count; x++)
            {
                if (currentValidPositions[x] != selfPosition)
                {
                    currentValidPositions.RemoveAt(x);
                    x--;
                }
            }
        }
    }
}
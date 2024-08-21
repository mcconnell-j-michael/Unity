using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public abstract class A_TargetHolder<T> : I_TargetHolder where T : A_TargetHolder<T>, new()
    {
        private List<PartyPosition> validPositions;
        private int[] threatMap;

        protected ToolManager source;
        protected A_PartyManager sourceParty;
        protected A_PartyManager targetParty;
        protected AbilityAction action;
        protected TargetingProcessor targetingProcessor;
        protected AbilityDeliveryPackProcessor deliveryProcessor;
        protected AbilityHitChanceProcessor hitChanceProcessor;
        protected AbilityAnimationProcessor animationProcessor;

        public virtual I_TargetHolder Clone()
        {
            return new T();
        }

        public void Initialize(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, AbilityAction action)
        {
            this.source = source;
            this.sourceParty = sourceParty;
            this.targetParty = targetParty;
            this.action = action;
            targetingProcessor = action.Get<TargetingProcessor>();
            deliveryProcessor = action.Get<AbilityDeliveryPackProcessor>();
            hitChanceProcessor = action.Get<AbilityHitChanceProcessor>();
            animationProcessor = action.Get<AbilityAnimationProcessor>();
            validPositions = null;
            Initialize();
        }

        protected virtual void Initialize()
        { }

        public void Cleanup()
        {
            targetParty.DeselectAll();
            CleanupInternal();
        }

        public void PopulateThreatMap()
        {
            if (threatMap == null)
            {
                threatMap = new int[PartyPositions.Count];
            }
            for (int x = 0; x < threatMap.Length; x++)
            {
                threatMap[x] = 0;
            }
            List<PartyPosition> validPositions = GetValidPositions();
            foreach (PartyPosition activePosition in targetParty.GetActivePositions())
            {
                if (validPositions.Contains(activePosition))
                {
                    ToolManager tm = targetParty.GetToolManager(activePosition);
                    AttributeTool at = tm.Get<AttributeTool>();
                    threatMap[(int)activePosition] = (int)at.GetAttribute(DerivedAttributes.Instance.threat);
                    threatMap[(int)activePosition] /= (activePosition.partyRow.rangePenalty + 1);
                }
            }
        }

        public PartyPosition GetPositionByThreat()
        {
            PopulateThreatMap();
            List<ThreatPack> threatPacks = new List<ThreatPack>();
            int totalThreat = 0;
            for (int x = 0; x < threatMap.Length; x++)
            {
                if (threatMap[x] > 0)
                {
                    totalThreat += threatMap[x];
                    ThreatPack pack = new ThreatPack()
                    {
                        threat = threatMap[x],
                        position = PartyPositions.Instance[x],
                    };
                    bool added = false;
                    for (int y = 0; y < threatPacks.Count; y++)
                    {
                        if (threatPacks[y].threat > pack.threat)
                        {
                            added = true;
                            threatPacks.Insert(y, pack);
                            break;
                        }
                    }
                    if (!added)
                    {
                        threatPacks.Add(pack);
                    }
                }
            }
            int threatChoice = Random.Range(0, totalThreat);
            for (int x = 0; x < threatPacks.Count; x++)
            {
                threatChoice -= threatPacks[x].threat;
                if (threatChoice < 0)
                {
                    return threatPacks[x].position;
                }
            }
            return null;
        }

        protected virtual void CleanupInternal() { }

        public virtual void InitializeTarget(I_Targetable targetable)
        {
        }

        public virtual I_Targetable GetFirstAvailableTargetable()
        {
            List<PartyPosition> vailidPositions = GetValidPositions();
            if (vailidPositions.Count == 0)
            {
                return null;
            }
            foreach (PartyPosition position in GetValidPositions())
            {
                I_Targetable targetable = targetParty.GetTargetable(position);
                if (targetable != null)
                {
                    return targetable;
                }
            }
            return null;
        }

        public List<PartyPosition> GetValidPositions()
        {
            if (validPositions != null)
            {
                return validPositions;
            }
            validPositions = new List<PartyPosition>();
            validPositions.AddRange(PartyPositions.Instance);
            TargetRange range = targetingProcessor.GetTargetRange(source);
            PartyPosition sourcePosition = sourceParty.GetPosition(source);

            if (sourceParty != targetParty || range.restrictSelfParty)
            {
                foreach (PartyPosition position in PartyPositions.Instance)
                {
                    if (!range.IsInRange(sourcePosition, position))
                    {
                        validPositions.Remove(position);
                    }
                }
            }
            foreach (I_TargetingRule rule in targetingProcessor.GetTargetingRules(source))
            {
                foreach (PartyPosition position in PartyPositions.Instance)
                {
                    if (validPositions.Contains(position) && !rule.IsValidTarget(source, targetParty.GetToolManager(position), position))
                    {
                        validPositions.Remove(position);
                    }
                }
            }
            TargetHolderRestrictions(validPositions);
            return validPositions;
        }

        protected virtual void TargetHolderRestrictions(List<PartyPosition> currentValidPositions) { }

        protected void UncacheValidPositions()
        {
            validPositions = null;
        }

        public virtual void SetTargetable(ToolManager target)
        {
            GetRandomTargetable();
        }

        public I_CombatProcessor ResolveTarget()
        {
            UncacheValidPositions();
            return ResolveTargetInternal();
        }

        public List<TargetResult> FinalizeTargets()
        {
            UncacheValidPositions();
            return FinalizeResultsInternal();
        }

        public bool HasNextTarget()
        {
            UncacheValidPositions();
            return HasNextTargetInternal() && GetValidPositions().Count > 0;
        }

        public float?[] GetDefaultEffectFloatArguments(PartyPosition target)
        {
            PartyPosition sourcePosition = sourceParty.GetPosition(source);
            float?[] effectFloatArguments = new float?[EffectFloatArguments.Count];
            effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] = 1f;
            if (sourceParty != targetParty)
            {
                TargetRange range = targetingProcessor.GetTargetRange(source);
                float multiplier = range.GetMultiplierForRange(sourcePosition, target);
                effectFloatArguments[(int)EffectFloatArguments.Instance.reservedDamageScale] = multiplier;
            }
            return effectFloatArguments;
        }

        protected PartyPosition GetRandomValidPosition()
        {
            List<PartyPosition> positions = GetValidPositions();
            if (positions.Count == 0)
            {
                return null;
            }
            int random = Random.Range(0, positions.Count);
            return positions[random];
        }

        protected PartyPosition GetRandomRowValidPosition(PartyRow row)
        {
            List<PartyPosition> positions = GetValidPositions();
            if (positions.Count == 0)
            {
                return null;
            }
            List<int> indexes = new List<int>();
            for (int x = 0; x < positions.Count; x++)
            {
                if (positions[x].partyRow == row)
                {
                    indexes.Add(x);
                }
            }
            if (indexes.Count == 0)
            {
                return null;
            }
            int random = Random.Range(0, indexes.Count);
            return positions[indexes[random]];
        }

        protected PartyPosition GetRandomOrderedRowValidPosition()
        {
            PartyRow first = PartyRows.Instance.First();
            PartyRow row = first;
            PartyPosition position = null;
            do
            {
                position = GetRandomRowValidPosition(row);
                row = first.backward;
            } while (row != first && position == null);
            return position;
        }

        public IEnumerable<PartyPosition> GetValidPositionsInRow(PartyRow row)
        {
            foreach (PartyPosition position in GetValidPositions())
            {
                if (position.partyRow == row)
                {
                    yield return position;
                }
            }
        }

        protected PartyPosition GetValidPosition(PartyPosition position, MoveDirection moveDirection)
        {
            return PartyPositions.Instance.GetValidPosition(GetValidPositions(), position, moveDirection, targetParty == sourceParty);
        }

        public void InitializeTarget(ToolManager target)
        {
            PartyPosition position = targetParty.GetPosition(target);
            InitializeTarget(targetParty.GetTargetable(position));
        }

        public abstract I_Targetable RequestMove(MoveDirection moveDirection);
        public abstract void GetRandomTargetable();
        public abstract I_CombatProcessor ResolveTargetInternal();
        public abstract bool HasNextTargetInternal();
        public abstract void GetTargetableByThreat();
        public abstract List<TargetResult> FinalizeResultsInternal();
    }

    struct ThreatPack
    {
        public PartyPosition position;
        public int threat;
    }
}
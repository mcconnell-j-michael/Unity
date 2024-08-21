using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetRange : A_EnumSO<TargetRange, TargetRanges>
{
    [SerializeField]
    private int range;

    [OdinSerialize]
    private Dictionary<int, float> rangeMultiplier;

    public bool restrictSelfParty;
    [SerializeField]
    private bool restrictColumn;
    [SerializeField]
    private bool restrictRow;
    [SerializeField]
    private bool squareDistance;

    [NonSerialized]
    private float[] multipliers;
    public float GetMultiplierForRange(PartyPosition source, PartyPosition target)
    {
        int requestedRange = CalculateRangePenalty(source, target);
        if (range < 0 || requestedRange > range)
        {
            return -1;
        }
        if (multipliers == null || multipliers.Length <= range)
        {
            multipliers = new float[range + 1];
            float current = 1f;
            for (int x = 0; x < (range + 1); x++)
            {
                if (rangeMultiplier != null && rangeMultiplier.ContainsKey(x))
                {
                    current = rangeMultiplier[x];
                }
                multipliers[x] = current;
            }
        }
        return multipliers[requestedRange];
    }

    public bool IsInRange(PartyPosition source, PartyPosition target)
    {
        return CalculateRangePenalty(source, target) <= range;
    }

    public int CalculateRangePenalty(PartyPosition source, PartyPosition target)
    {
        int rangedPenalty = 0;
        int rowPenalty = 0;
        int columnPenalty = 0;
        if (restrictRow)
        {
            rowPenalty += (source.partyRow.rangePenalty + target.partyRow.rangePenalty);
        }
        if (restrictColumn)
        {
            columnPenalty += source.partyColumn.CalculateRangeDistance(target.partyColumn);
        }
        if (squareDistance)
        {
            rangedPenalty = Mathf.Max(rowPenalty, columnPenalty);
        }
        else
        {
            rangedPenalty = rowPenalty + columnPenalty;
        }
        return rangedPenalty;
    }
}

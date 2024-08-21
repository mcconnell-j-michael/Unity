using UnityEngine;

public class PartyColumn : A_EnumSO<PartyColumn, PartyColumns>
{
    public int CalculateRangeDistance(PartyColumn column)
    {
        return Mathf.Abs(column.index - index);
    }
}
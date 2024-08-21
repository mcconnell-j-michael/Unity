using Ashen.EnumSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyPositions : A_DependentEnumList<PartyPosition, PartyPositions, PartyRow, PartyRows, PartyColumn, PartyColumns>
{
    //protected override List<PartyPosition> GetEnumList()
    //{
    //    if (!Application.isPlaying)
    //    {
    //        return enumList;
    //    }
    //    disableAutoChecks = true;
    //    if (enumList == null)
    //    {
    //        enumList = new List<PartyPosition>();
    //    }
    //    int total = PartyRows.Count * PartyColumns.Count;
    //    if (enumList.Count != total)
    //    {
    //        List<PartyPosition> sparePositions = new();
    //        while (enumList.Count < total)
    //        {
    //            enumList.Add(null);
    //        }
    //        for (int x = 0; x < enumList.Count; x++)
    //        {
    //            PartyPosition pos = enumList[x];
    //            if (pos)
    //            {
    //                if (!pos.partyRow || !pos.partyColumn)
    //                {
    //                    sparePositions.Add(pos);
    //                    enumList[x] = null;
    //                    continue;
    //                }
    //                int trueIndex = GetIndex(pos.partyRow, pos.partyColumn);
    //                if (x != trueIndex)
    //                {
    //                    PartyPosition swapPos = enumList[trueIndex];
    //                    if (!swapPos)
    //                    {
    //                        enumList[trueIndex] = pos;
    //                        enumList[x] = null;
    //                        continue;
    //                    }
    //                    if ((!swapPos.partyRow || !swapPos.partyColumn) || (swapPos.partyRow == pos.partyRow && swapPos.partyColumn == pos.partyColumn))
    //                    {
    //                        sparePositions.Add(swapPos);
    //                        enumList[trueIndex] = pos;
    //                        enumList[x] = null;
    //                        continue;
    //                    }
    //                    enumList[trueIndex] = pos;
    //                    enumList[x] = swapPos;
    //                    x--;
    //                    continue;
    //                }
    //            }
    //        }
    //        int totalColumns = PartyColumns.Count;
    //        int totalRows = PartyRows.Count;
    //        for (int x = 0; x < PartyRows.Count; x++)
    //        {
    //            for (int y = 0; y < PartyColumns.Count; y++)
    //            {
    //                int index = (totalColumns * x) + y;
    //                if (enumList[index])
    //                {
    //                    continue;
    //                }
    //                PartyPosition pos = null;
    //                if (sparePositions.Count > 0)
    //                {
    //                    pos = sparePositions[0];
    //                    sparePositions.RemoveAt(0);
    //                }
    //                else
    //                {
    //                    pos = CreateInstance<PartyPosition>();
    //                }
    //                pos.partyRow = PartyRows.Instance[x];
    //                pos.partyColumn = PartyColumns.Instance[y];
    //                pos.Index = index;
    //                pos.name = pos.partyRow.name + "_" + pos.partyColumn.name;
    //                enumList[index] = pos;
    //            }
    //        }
    //        for (int x = 0; x < PartyRows.Count; x++)
    //        {
    //            for (int y = 0; y < PartyColumns.Count; y++)
    //            {
    //                int index = (totalColumns * x) + y;
    //                PartyPosition pos = enumList[index];
    //                pos.Index = index;
    //                int xUp = x - 1;
    //                if (xUp < 0)
    //                {
    //                    xUp += totalRows;
    //                }
    //                int yLeft = y - 1;
    //                if (yLeft < 0)
    //                {
    //                    yLeft += totalColumns;
    //                }
    //                int upIndex = (totalColumns * xUp) + y;
    //                int downIndex = (totalColumns * ((x + 1) % totalRows)) + y;
    //                int leftIndex = (totalColumns * x) + yLeft;
    //                int rightIndex = (totalColumns * x) + ((y + 1) % totalColumns);
    //                PartyPosition upPos = enumList[upIndex];
    //                PartyPosition downPos = enumList[downIndex];
    //                PartyPosition leftPos = enumList[leftIndex];
    //                PartyPosition rightPos = enumList[rightIndex];
    //                pos.up = upPos;
    //                pos.down = downPos;
    //                pos.left = leftPos;
    //                pos.right = rightPos;
    //            }
    //        }
    //    }
    //    disableAutoChecks = false;
    //    return enumList;
    //}

    protected override void GetEnumListCleanup()
    {
        int totalColumns = PartyColumns.Count;
        int totalRows = PartyRows.Count;
        for (int x = 0; x < PartyRows.Count; x++)
        {
            for (int y = 0; y < PartyColumns.Count; y++)
            {
                int index = (totalColumns * x) + y;
                PartyPosition pos = GetEnumList()[index];
                pos.Index = index;
                int xUp = x - 1;
                if (xUp < 0)
                {
                    xUp += totalRows;
                }
                int yLeft = y - 1;
                if (yLeft < 0)
                {
                    yLeft += totalColumns;
                }
                int upIndex = (totalColumns * xUp) + y;
                int downIndex = (totalColumns * ((x + 1) % totalRows)) + y;
                int leftIndex = (totalColumns * x) + yLeft;
                int rightIndex = (totalColumns * x) + ((y + 1) % totalColumns);
                PartyPosition upPos = GetEnumList()[upIndex];
                PartyPosition downPos = GetEnumList()[downIndex];
                PartyPosition leftPos = GetEnumList()[leftIndex];
                PartyPosition rightPos = GetEnumList()[rightIndex];
                pos.up = upPos;
                pos.down = downPos;
                pos.left = leftPos;
                pos.right = rightPos;
            }
        }
    }

    public PartyPosition GetPartyPosition(PartyRow row, PartyColumn column)
    {
        int index = GetIndex(row, column);
        return GetEnumList()[index];
    }

    public PartyPosition GetValidPosition(List<PartyPosition> validPositions, PartyPosition position, MoveDirection moveDirection, bool inverseVertical)
    {
        if (moveDirection == MoveDirection.None)
        {
            return position;
        }
        PartyPosition next = null;
        int count = 0;
        PartyPosition cur = position;
        switch (moveDirection)
        {
            case MoveDirection.Left:
                count = 0;
                while (next == null && count < Count)
                {
                    cur = cur.left;
                    if (validPositions.Contains(cur))
                    {
                        next = cur;
                    }
                    count++;
                }
                break;
            case MoveDirection.Right:
                count = 0;
                while (next == null && count < Count)
                {
                    cur = cur.right;
                    if (validPositions.Contains(cur))
                    {
                        next = cur;
                    }
                    count++;
                }
                break;
            case MoveDirection.Up:
                if (inverseVertical)
                {
                    next = HandleVertical(validPositions, position, MoveDirection.Down);
                }
                else
                {
                    next = HandleVertical(validPositions, position, MoveDirection.Up);
                }
                break;
            case MoveDirection.Down:
                if (inverseVertical)
                {
                    next = HandleVertical(validPositions, position, MoveDirection.Up);
                }
                else
                {
                    next = HandleVertical(validPositions, position, MoveDirection.Down);
                }
                break;
        }
        if (next == null)
        {
            return position;
        }
        return next;
    }

    private PartyPosition HandleVertical(List<PartyPosition> validPositions, PartyPosition position, MoveDirection moveDirection)
    {
        PartyPosition cur = MoveVertical(position, moveDirection);
        PartyPosition right = null;
        PartyPosition next = null;
        int count = 0;
        while (right == null && count < Count)
        {
            cur = cur.right;
            if (validPositions.Contains(cur))
            {
                right = MoveVertical(cur, moveDirection);
            }
            count++;
        }
        count = 0;
        PartyPosition left = null;
        while (left == null && count < Count)
        {
            cur = cur.left;
            if (validPositions.Contains(cur))
            {
                left = MoveVertical(cur, moveDirection);
            }
            count++;
        }

        if (right == null && left == null)
        {
            return next;
        }
        else if (right == null)
        {
            next = MoveVertical(left, moveDirection);
            return next;
        }
        else if (left == null)
        {
            next = MoveVertical(right, moveDirection);
            return next;
        }

        if (Mathf.Abs(right.Index - position.Index) <= Mathf.Abs(left.Index - position.Index))
        {
            next = MoveVertical(right, moveDirection);
        }
        else
        {
            next = MoveVertical(left, moveDirection);
        }
        return next;
    }

    public PartyPosition MoveVertical(PartyPosition position, MoveDirection moveDirection)
    {
        return (MoveDirection.Up == moveDirection) ? position.up : position.down;
    }

    //private int GetIndex(PartyRow row, PartyColumn column)
    //{
    //    return (((int)row) * PartyColumns.Count) + ((int)column);
    //}
}

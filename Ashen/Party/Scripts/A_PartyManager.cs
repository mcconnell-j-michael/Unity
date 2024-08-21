using Ashen.AbilitySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

public abstract class A_PartyManager : SerializedMonoBehaviour
{
    public ToolManager partyToolManager;

    [ShowInInspector]
    protected ToolManager[] toolManagerPosition;

    [EnumSODropdown]
    public List<PartyRow> enabledRows;
    [EnumSODropdown]
    public List<PartyColumn> enabledColumns;

    [NonSerialized]
    public List<PartyPosition> enabledPositions;

    [NonSerialized]
    private A_PartyUIManager partyUIManager;

    private BattleContainer currentBattleContainer;

    private void Awake()
    {
        partyToolManager = GetComponent<ToolManager>();
        InitializePartyTools();
        toolManagerPosition = new ToolManager[PartyPositions.Count];

        enabledPositions = new List<PartyPosition>();
        foreach (PartyRow row in enabledRows)
        {
            foreach (PartyColumn col in enabledColumns)
            {
                enabledPositions.Add(PartyPositions.Instance.GetPartyPosition(row, col));
            }
        }
    }

    protected virtual void InitializePartyTools() { }

    protected virtual void Start()
    {
        ToolManager[] managers = GetComponentsInChildren<ToolManager>();
        int index = 0;
        foreach (PartyRow row in enabledRows)
        {
            if (index >= managers.Length)
            {
                break;
            }
            foreach (PartyColumn col in enabledColumns)
            {
                if (index >= managers.Length)
                {
                    break;
                }
                PartyPosition pos = PartyPositions.Instance.GetPartyPosition(row, col);
                SetToolManager(pos, managers[index]);
                index++;
            }
        }
    }

    public void RegisterBattleContainer(BattleContainer currentBattleContainer)
    {
        this.currentBattleContainer = currentBattleContainer;
    }

    public BattleContainer GetCurrentBattleContainer()
    {
        return currentBattleContainer;
    }

    public ToolManager GetFirst()
    {
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)partyPosition] != null)
            {
                return toolManagerPosition[(int)partyPosition];
            }
        }
        return null;
    }

    public ToolManager GetLast()
    {
        ToolManager lastFound = null;
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (toolManagerPosition[(int)partyPosition] != null)
            {
                lastFound = toolManagerPosition[(int)partyPosition];
            }
        }
        return lastFound;
    }

    public PartyPosition GetNextTargetableCharacterInRow(PartyPosition position)
    {
        bool found = false;
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (partyPosition == position)
            {
                found = true;
                continue;
            }
            if (!found)
            {
                continue;
            }
            if (partyPosition.partyRow == position.partyRow && toolManagerPosition[(int)partyPosition] != null)
            {
                return partyPosition;
            }
        }
        return null;
    }

    public PartyPosition GetPreviousTargetableCharacterInRow(PartyPosition position)
    {
        PartyPosition previous = null;
        foreach (PartyPosition partyPosition in PartyPositions.Instance)
        {
            if (partyPosition == position)
            {
                break;
            }
            if (partyPosition.partyRow == position.partyRow && toolManagerPosition[(int)partyPosition] != null)
            {
                previous = partyPosition;
            }
        }
        return previous;
    }

    public ToolManager GetNext(ToolManager toolManager)
    {
        PartyPosition position = GetPosition(toolManager);
        for (int x = (int)position + 1; x < PartyPositions.Count; x++)
        {
            if (toolManagerPosition[x] != null)
            {
                return toolManagerPosition[x];
            }
        }
        return null;
    }

    public ToolManager GetPrevious(ToolManager toolManager)
    {
        PartyPosition position = GetPosition(toolManager);
        for (int x = (int)position - 1; x >= 0; x--)
        {
            if (toolManagerPosition[x] != null)
            {
                return toolManagerPosition[x];
            }
        }
        return null;
    }

    [Button]
    public void SwapRows(PartyRow row1, PartyRow row2)
    {
        if (row1 == row2 || row1 == null || row2 == null)
        {
            return;
        }

        foreach (PartyColumn col in enabledColumns)
        {
            PartyPosition posR1 = PartyPositions.Instance.GetPartyPosition(row1, col);
            PartyPosition posR2 = PartyPositions.Instance.GetPartyPosition(row2, col);

            Swap(posR1, posR2);
        }

        Refresh();
    }

    protected virtual void Swap(PartyPosition pos1, PartyPosition pos2)
    {
        ToolManager tm1 = GetToolManager(pos1);
        ToolManager tm2 = GetToolManager(pos2);

        SetToolManager(pos1, tm2);
        SetToolManager(pos2, tm1);
    }

    public virtual void SetToolManager(PartyPosition position, ToolManager toolManager)
    {
        toolManagerPosition[(int)position] = toolManager;
        Refresh();
    }

    public ToolManager GetToolManager(PartyPosition position)
    {
        return toolManagerPosition[(int)position];
    }

    public I_Targetable GetTargetable(PartyPosition position)
    {
        return partyUIManager.GetTargetable(position);
    }

    public void DeselectAll()
    {
        partyUIManager.DeselectAll();
    }

    public void RegisterPartyUIManager(A_PartyUIManager manager)
    {
        partyUIManager = manager;
    }

    public A_PartyUIManager GetPartyUIManager()
    {
        return partyUIManager;
    }

    public PartyPosition GetFirstEnabledPartyPosition()
    {
        if (enabledPositions == null || enabledPositions.Count == 0)
        {
            return null;
        }
        return enabledPositions[0];
    }

    public PartyPosition GetPosition(ToolManager toolManager)
    {
        if (toolManager == null)
        {
            return null;
        }
        for (int x = 0; x < toolManagerPosition.Length; x++)
        {
            if (toolManagerPosition[x] == null)
            {
                continue;
            }
            if (ReferenceEquals(toolManager.gameObject, toolManagerPosition[x].gameObject))
            {
                return PartyPositions.Instance[x];
            }
        }
        return null;
    }

    public void Refresh()
    {
        if (partyUIManager != null)
        {
            foreach (PartyPosition position in enabledPositions)
            {
                if (partyUIManager.GetTargetable(position).GetTarget() != GetToolManager(position))
                {
                    partyUIManager.SetPartyMember(position, GetToolManager(position));
                }
            }
        }
    }

    public IEnumerable<PartyPosition> GetActivePositions()
    {
        for (int x = 0; x < PartyPositions.Count; x++)
        {
            if (toolManagerPosition[x] != null)
            {
                yield return PartyPositions.Instance[x];
            }
        }
    }
}

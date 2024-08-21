using Ashen.AbilitySystem;
using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_PartyUIManager : MonoBehaviour
{
    [NonSerialized]
    public Dictionary<PartyPosition, A_CharacterSelector> positionToManager;

    protected A_CharacterSelector[] managers;

    protected virtual void Start()
    {
        managers = new A_CharacterSelector[PartyPositions.Count];
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (positionToManager.TryGetValue(position, out A_CharacterSelector manager))
            {
                managers[(int)position] = manager;
            }
        }
        A_PartyManager partyManager = GetPartyManager();
    }

    protected abstract A_PartyManager GetPartyManager();

    public virtual void SetPartyMember(PartyPosition position, ToolManager toolManager)
    {
        if (managers[(int)position])
        {
            managers[(int)position].RegisterToolManager(toolManager);
        }
    }

    public void CleanUp()
    {
        foreach (PartyPosition position in PartyPositions.Instance)
        {
            if (managers[(int)position] != null)
            {
                managers[(int)position].UnregisterToolManager();
            }
        }
    }

    public I_Targetable GetTargetable(PartyPosition position)
    {
        return managers[(int)position];
    }

    public A_CharacterSelector GetCharacterSelector(PartyPosition position)
    {
        return managers[(int)position];
    }

    public void DeselectAll()
    {
        foreach (I_Targetable targetable in managers)
        {
            if (targetable != null)
            {
                targetable.Deselected();
            }
        }
    }
}

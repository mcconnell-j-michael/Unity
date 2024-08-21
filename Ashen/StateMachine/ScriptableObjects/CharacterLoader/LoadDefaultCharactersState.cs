using Ashen.ToolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerPartyManager;

public class LoadDefaultCharactersState : I_GameState
{
    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        PlayerPartyManager manager = PlayerPartyHolder.Instance.partyManager;
        UnitManager unitManager = UnitManager.Instance;
        DefaultCharacter[] configs = manager.startingCharacters;
        int rowIdx = 0;
        int columnIdx = 0;
        foreach (DefaultCharacter config in configs)
        {
            GameObject character = Object.Instantiate(manager.defaultCharacterPrefab, unitManager.transform);
            character.name = config.config.className;
            ToolManager tm = character.GetComponent<ToolManager>();
            SavableCharacter savableCharacter = character.GetComponent<SavableCharacter>();
            if (!savableCharacter)
            {
                character.AddComponent<SavableCharacter>();
            }
            unitManager.tms.Add(tm);
            config.config.BuildCharacter(tm, new Dictionary<string, object>()
            {
                ["subclass"] = config.subclass
            });
            PartyRow row = manager.enabledRows[rowIdx];
            PartyColumn col = manager.enabledColumns[columnIdx];
            PartyPosition pos = PartyPositions.Instance.GetPartyPosition(row, col);
            manager.SetToolManager(pos, tm);

            columnIdx += 1;
            if (columnIdx >= manager.enabledColumns.Count)
            {
                columnIdx = 0;
                rowIdx += 1;
                if (rowIdx >= manager.enabledRows.Count)
                {
                    break;
                }
            }
        }
        yield break;
    }
}

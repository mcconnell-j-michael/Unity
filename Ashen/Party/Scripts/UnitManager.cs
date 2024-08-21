using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : SingletonMonoBehaviour<UnitManager>, I_Saveable
{
    public List<ToolManager> tms;

    public GameObject baseCharacter;

    public void Start()
    {
        tms = new List<ToolManager>();
    }

    public object CaptureState()
    {
        PlayerPartyManager manager = PlayerPartyHolder.Instance.partyManager;
        SavableCharacter[] characters = GetComponentsInChildren<SavableCharacter>();
        CharacterSaver[] characterSavers = new CharacterSaver[characters.Length];
        for (int x = 0; x < characters.Length; x++)
        {
            ToolManager tm = characters[x].GetComponent<ToolManager>();
            PartyPosition pos = manager.GetPosition(tm);
            ConfigurationTool configurationTool = tm.Get<ConfigurationTool>();
            characterSavers[x] = new CharacterSaver()
            {
                name = tm.gameObject.name,
                configId = configurationTool.GetConfigurationValues().name,
                partyPosition = pos.Index,
                characterInfo = characters[x].CaptureState()
            };
        }
        return new CharacterSavers()
        {
            characters = characterSavers
        };
    }

    public void RestoreState(object state)
    {
        PlayerPartyManager manager = PlayerPartyHolder.Instance.partyManager;
        foreach (PartyPosition pos in manager.GetActivePositions())
        {
            manager.SetToolManager(pos, null);
        }
        SavableCharacter[] oldCharacters = GetComponentsInChildren<SavableCharacter>();
        foreach (SavableCharacter character in oldCharacters)
        {
            Destroy(character.gameObject);
        }

        CharacterSaver[] characters = ((CharacterSavers)state).characters;
        ConfigurationLoader loader = ConfigurationLoader.Instance;

        foreach (CharacterSaver character in characters)
        {
            ConfigurationValues config = loader.GetScriptableObject(character.configId);
            GameObject characterGO = Instantiate(PlayerPartyHolder.Instance.partyManager.defaultCharacterPrefab, gameObject.transform);
            characterGO.name = character.name;
            ToolManager tm = characterGO.GetComponent<ToolManager>();
            tms.Add(tm);
            config.BuildCharacter(tm);
            if (character.partyPosition != null)
            {
                PartyPosition pos = PartyPositions.Instance[(int)character.partyPosition];
                manager.SetToolManager(pos, tm);
            }
        }
    }

    public void PrepareRestoreState()
    {
        tms.Clear();
    }

    [Serializable]
    private struct CharacterSavers
    {
        public CharacterSaver[] characters;
    }

    [Serializable]
    private struct CharacterSaver
    {
        public string name;
        public string configId;
        public int? partyPosition;
        public Dictionary<string, object> characterInfo;
    }
}

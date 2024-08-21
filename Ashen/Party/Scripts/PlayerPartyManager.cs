using Ashen.PartySystem;
using Ashen.SkillTreeSystem;
using Ashen.ToolSystem;
using UnityEngine;

public class PlayerPartyManager : A_PartyManager
{
    public GameObject defaultCharacterPrefab;
    public DefaultCharacter[] startingCharacters;

    private ConfigurationValues configuration;
    private ConfigurationValues Configuration
    {
        get
        {
            if (!configuration)
            {
                return DefaultValues.Instance.config;
            }
            return configuration;
        }
    }

    protected override void InitializePartyTools()
    {
        Configuration.BuildCharacter(partyToolManager);
        //if (!GetComponent<PartyItemsManager>())
        //{
        //    PartyItemsManager manager = gameObject.AddComponent<PartyItemsManager>();
        //    manager.Initialize(Configuration.GetConfiguration<PartyItemConfiguration>());
        //}
        if (!GetComponent<PartyResourceTracker>())
        {
            PartyResourceTracker partyResourceTracker = gameObject.AddComponent<PartyResourceTracker>();
        }
        //if (!GetComponent<ResourceValueTool>())
        //{
        //    ResourceValueTool resourceValueTool = gameObject.AddComponent<ResourceValueTool>();
        //}
        //if (!GetComponent<)
    }

    protected override void Start()
    {
        base.Start();
        PlayerPartyHolder.Instance.partyManager = this;
    }

    public override void SetToolManager(PartyPosition position, ToolManager toolManager)
    {
        base.SetToolManager(position, toolManager);
    }

    public struct DefaultCharacter
    {
        public ConfigurationValues config;
        public SubSkillTreeKey subclass;
    }
}

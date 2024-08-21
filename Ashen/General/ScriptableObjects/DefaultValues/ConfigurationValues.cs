using Ashen.PartySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

public class ConfigurationValues : SerializedScriptableObject
{
    public string className;

    public PartyItemConfiguration partyItemConfiguration;

    public List<I_Configuration> configs;

    [HideIf("@" + nameof(IsDefault) + "()")]
    public List<I_Configuration> additionalConfigs;

    public void BuildCharacter(ToolManager manager, Dictionary<string, object> arguments = null)
    {
        List<I_Configuration> configurations = DefaultValues.Instance.config.configs;
        foreach (I_Configuration config in configurations)
        {
            I_Configuration configToUse = GetConfiguration(config.GetType());
            if (configToUse == null)
            {
                configToUse = config;
            }
            configToUse.Reconfigure(manager, arguments);
        }
        if (additionalConfigs != null)
        {
            foreach (I_Configuration config in additionalConfigs)
            {
                if (config != null)
                {
                    config.Reconfigure(manager, arguments);
                }
            }
        }
    }

    public T GetConfiguration<T>() where T : class, I_Configuration
    {
        if (configs == null)
        {
            return null;
        }
        foreach (I_Configuration config in configs)
        {
            if (config.GetType() == typeof(T))
            {
                return config as T;
            }
        }
        return null;
    }

    private I_Configuration GetConfiguration(Type type)
    {
        if (configs == null)
        {
            return null;
        }
        foreach (I_Configuration config in configs)
        {
            if (config.GetType() == type)
            {
                return config;
            }
        }
        return null;
    }

    private bool IsDefault()
    {
        return this == DefaultValues.Instance.config;
    }
}

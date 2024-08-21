using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultValues", menuName = "Custom/CombatInfrastructure/DefaultValues")]
public class DefaultValues : SingletonScriptableObject<DefaultValues>
{
    public ConfigurationValues config;

#if UNITY_EDITOR
    [Button]
    private void BuildConfigurations()
    {
        config.configs = new List<I_Configuration>();
        List<I_Configuration> foundConfigurations = StaticUtilities.FindAssetsByTypeGeneral<I_Configuration>();
        foreach (I_Configuration configuration in foundConfigurations)
        {
            if (configuration.IsDefault())
            {
                config.configs.Add(configuration);
            }
        }
    }
#endif
}

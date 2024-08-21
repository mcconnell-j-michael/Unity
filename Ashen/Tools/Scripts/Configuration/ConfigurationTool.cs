namespace Ashen.ToolSystem
{
    public class ConfigurationTool : A_ConfigurableTool<ConfigurationTool, ConfigurationToolConfiguration>
    {
        private ConfigurationValues configValue;

        public ConfigurationValues GetConfigurationValues()
        {
            if (configValue)
            {
                return configValue;
            }
            return DefaultValues.Instance.config;
        }

        public void SetConfigurationValues(ConfigurationValues configValue)
        {
            this.configValue = configValue;
        }
    }
}
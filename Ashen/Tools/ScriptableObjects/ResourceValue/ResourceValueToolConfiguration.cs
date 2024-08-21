using UnityEngine;

namespace Ashen.ToolSystem
{
    [CreateAssetMenu(fileName = "ResourceValueToolConfiguration", menuName = "Custom/Tool/ResourceValueToolConfiguration")]
    public class ResourceValueToolConfiguration : A_Configuration<ResourceValueTool, ResourceValueToolConfiguration>
    {
        [SerializeField]
        private AbilityResourceConfig abilityResourceConfig;

        public AbilityResourceConfig AbilityResourceConfig
        {
            get
            {
                if (!abilityResourceConfig)
                {
                    if (this == GetDefault())
                    {
                        return null;
                    }
                    return GetDefault().abilityResourceConfig;
                }
                return abilityResourceConfig;
            }
        }
    }
}
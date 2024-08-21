using UnityEngine;

namespace Ashen.ToolSystem
{
    [CreateAssetMenu(fileName = nameof(ResourceValues), menuName = "Custom/Enums/" + nameof(ResourceValues) + "/Types")]
    public class ResourceValues : A_EnumList<ResourceValue, ResourceValues>
    {
        public ResourceValue health;
        public ResourceValue CONCENTRATION;
        public ResourceValue ABILITY_RESOURCE;
        public ResourceValue ACTION_POINT;
    }
}
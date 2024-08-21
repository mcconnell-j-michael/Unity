using UnityEngine;

namespace Ashen.ToolSystem
{
    public class BasicCharacter : MonoBehaviour
    {
        public ConfigurationValues config;

        private void Awake()
        {
            ToolManager toolManager = gameObject.GetComponent<ToolManager>();
            if (!toolManager)
            {
                toolManager = gameObject.AddComponent<ToolManager>();
            }
            config.BuildCharacter(toolManager);
        }
    }
}

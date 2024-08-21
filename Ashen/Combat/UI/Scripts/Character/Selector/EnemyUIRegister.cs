using Ashen.ToolSystem;
using UnityEngine;

public class EnemyUIRegister : MonoBehaviour
{
    public ResourceBarManager healthBar;
    public ToolManager toolManager;

    public void Start()
    {
        healthBar.RegisterToolManager(toolManager);
    }

    public void OnDestroy()
    {
        if (healthBar)
        {
            healthBar.UnregisterToolManager();
        }
    }
}

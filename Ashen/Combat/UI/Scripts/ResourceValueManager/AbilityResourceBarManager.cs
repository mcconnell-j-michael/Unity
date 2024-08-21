using Ashen.ToolSystem;

namespace Ashen.CombatSystem
{
    public class AbilityResourceBarManager : NamedResourceBarManager
    {
        protected override void InternalRegisterToolManager(ToolManager toolManager)
        {
            base.InternalRegisterToolManager(toolManager);
            ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
            AbilityResourceConfig abilityResourceConfig = resourceValueTool.AbilityResourceConfig;
            gradient.LinearColor1 = abilityResourceConfig.ResourceColor1;
            gradient.LinearColor2 = abilityResourceConfig.ResourceColor2;
            resourceNameText.text = abilityResourceConfig.ResourceDisplayName;
        }

        protected override void InternalUnregisterToolManager()
        {
            base.InternalUnregisterToolManager();
            resourceNameText.text = "";
        }
    }
}
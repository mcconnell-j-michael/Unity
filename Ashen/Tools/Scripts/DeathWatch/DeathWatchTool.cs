using Ashen.DeliverySystem;

namespace Ashen.ToolSystem
{
    public class DeathWatchTool : A_ConfigurableTool<DeathWatchTool, DeathWatchToolConfiguration>, I_ThresholdListener
    {
        private DeliveryPackScriptableObject onDeath;

        public override void Initialize()
        {
            base.Initialize();

            onDeath = Config.onHealthReduced;
        }

        private void Start()
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            rvTool.UnRegesterThresholdMetListener(ResourceValues.Instance.health, this);
            rvTool.RegisterThresholdMetListener(ResourceValues.Instance.health, this);
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            DeliveryTool deliveryTool = toolManager.Get<DeliveryTool>();
            deliveryTool.Deliver(onDeath, toolManager);
        }
    }
}
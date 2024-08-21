using Ashen.DeliverySystem;
using Sirenix.OdinInspector;

namespace Ashen.ToolSystem
{
    public class LevelTool : A_ConfigurableTool<LevelTool, LevelToolConfiguration>, I_ThresholdListener
    {
        private BaseAttributeTool baTool;

        [ShowInInspector]
        private ResourceValue experienceResourceValue = default;
        [ShowInInspector]
        private BaseAttribute levelBaseAttribute = default;

        public override void Initialize()
        {
            base.Initialize();
            experienceResourceValue = Config.ExperienceResourceValue;
            levelBaseAttribute = Config.LevelBaseAttribute;
        }

        public void Start()
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            rvTool.RegisterThresholdMetListener(experienceResourceValue, this);
            baTool = toolManager.Get<BaseAttributeTool>();
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            baTool.AddBase(levelBaseAttribute, 1);
        }
    }
}
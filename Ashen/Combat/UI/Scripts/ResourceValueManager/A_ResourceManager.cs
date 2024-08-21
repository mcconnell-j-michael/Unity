using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public abstract class A_ResourceManager : SerializedMonoBehaviour, I_ThresholdListener, I_ResourceManagerUI
    {
        [SerializeField]
        protected ResourceValue resourceValue;

        protected ToolManager toolManager;

        public void RegisterToolManager(ToolManager toolManager)
        {
            UnregisterToolManager();
            this.toolManager = toolManager;
            ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
            resourceValueTool.RegiserThresholdChangeListener(resourceValue, this);
            InternalRegisterToolManager(toolManager);
        }

        public void UnregisterToolManager()
        {
            if (this.toolManager)
            {
                ResourceValueTool oldValueTool = this.toolManager.Get<ResourceValueTool>();
                oldValueTool.UnRegesterThresholdChangeListener(resourceValue, this);
                InternalUnregisterToolManager();
                toolManager = null;
            }
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            UpdateFill(value);
        }

        [Button]
        private void OnEnable()
        {
            if (toolManager)
            {
                ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
                OnThresholdEvent(rvTool.GetValue(resourceValue));
            }
        }

        protected virtual void InternalRegisterToolManager(ToolManager toolManager) { }
        protected virtual void InternalUnregisterToolManager() { }
        protected abstract void UpdateFill(ThresholdEventValue value);
    }
}
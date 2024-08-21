using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.ToolSystem;

namespace Ashen.ExtendedEffectSystem
{
    public class DerivedAttributeListenerComponent : A_ListenerComponent<DerivedAttribute>, I_EnumCacheable
    {
        public DerivedAttributeListenerComponent(DerivedAttribute listenOn, I_ExtendedEffectBuilder statusEffect) : base(listenOn, statusEffect)
        {
        }

        public DerivedAttributeListenerComponent() : base() { }

        public void Recalculate(I_EnumSO enumValue, I_DeliveryTool toolManager)
        {
            OnListenerTrigger();
        }

        protected override void Register(I_DeliveryTool dt, DerivedAttribute toRegister)
        {
            ToolManager toolManager = (dt as DeliveryTool).toolManager;
            AttributeTool at = toolManager.Get<AttributeTool>();
            at.Cache(toRegister, this);
        }

        protected override void Unregister(I_DeliveryTool dt, DerivedAttribute toUnRegister)
        {
            ToolManager toolManager = (dt as DeliveryTool).toolManager;
            AttributeTool at = toolManager.Get<AttributeTool>();
            at.UnCache(toUnRegister, this);
        }
    }
}

using Ashen.EnumSystem;
using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public abstract class A_DeliveryValue : I_DeliveryValue
    {
        public abstract float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
        public abstract string Visualize();
        public virtual void OnDeregister(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO) { }
        public virtual void OnRegister(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO) { }
    }
}
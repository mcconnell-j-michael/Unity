using Ashen.EnumSystem;
using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public interface I_DeliveryValue
    {
        float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
        string Visualize();
        void OnRegister(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO);
        void OnDeregister(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO);
    }
}

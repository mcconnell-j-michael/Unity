using Ashen.DeliverySystem;
using Ashen.EnumSystem;

namespace Ashen.ToolSystem
{
    public interface I_Cacher<Enum>
        where Enum : I_EnumSO
    {
        I_DeliveryTool GetDeliveryTool();
        void PreOnChange(Enum enumSO) { }
        void PostOnChange(Enum enumSO) { }
    }
}
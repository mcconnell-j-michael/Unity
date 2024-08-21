using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.ToolSystem;

namespace Ashen.EquationSystem
{
    public interface I_EquationAttribute<Tool, Enum, ReturnValue>
        where Tool : A_EnumeratedTool<Tool>, I_CacheableTool<Enum, ReturnValue>
        where Enum : I_EnumSO, I_EquationAttribute<Tool, Enum, ReturnValue>
    {
        public float Get(I_DeliveryTool deliveryTool, Enum enumValue, DeliveryArgumentPacks arguments)
        {
            DeliveryTool dTool = deliveryTool as DeliveryTool;
            if (!dTool)
            {
                return 1;
            }
            Tool fTool = dTool.toolManager.Get<Tool>();
            if (!fTool)
            {
                return 1;
            }
            return GetAsFloat(fTool.Get(enumValue, arguments));
        }

        public float GetAsFloat(ReturnValue value);
    }
}
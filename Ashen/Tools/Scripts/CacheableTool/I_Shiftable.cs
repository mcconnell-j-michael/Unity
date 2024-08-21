using Ashen.DeliverySystem;
using Ashen.EnumSystem;

namespace Ashen.ToolSystem
{
    public interface I_Shiftable<Enum, Shift> where Enum : I_EnumSO
    {
        void AddShift(Enum enumValue, int priority, ShiftCategory shiftCategory, string source, Shift value);
        void RemoveShift(Enum enumValue, ShiftCategory shiftCategory, string source);
    }
}
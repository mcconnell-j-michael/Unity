using Ashen.EnumSystem;

namespace Ashen.ToolSystem
{
    public interface I_Retrievable<Enum, Current>
        where Enum : I_EnumSO
    {
        Current GetAttribute(Enum enumValue);
        Current Get(Enum enumValue, DeliveryArgumentPacks argument);
        Current GetAttribute(Enum enumValue, AttributeLimiter limiter);
        Current GetBaseValue(Enum enumValue);
    }
}
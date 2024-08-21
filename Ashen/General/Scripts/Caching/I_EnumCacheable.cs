using Ashen.DeliverySystem;
using Ashen.EnumSystem;

public interface I_EnumCacheable
{
    void Recalculate(I_EnumSO enumValue, I_DeliveryTool deliveryTool);
}

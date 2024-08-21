using Ashen.ToolSystem;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    /**
     * The interface that all filters must implement. A filter must be able to apply itself and report whether it is still enabled
     **/
    public interface I_Filter
    {
        bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult);
        bool Enabled();
    }
}

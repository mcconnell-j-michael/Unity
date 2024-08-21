using Ashen.DeliverySystem;
using Sirenix.OdinInspector;

namespace Ashen.EquationSystem
{
    [HideReferenceObjectPicker]
    public interface I_Equation
    {
        float Calculate(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks equationArguments);
        float Calculate(I_DeliveryTool source, DeliveryArgumentPacks equationArguments);
        float Calculate(I_DeliveryTool source);
        bool RequiresRebuild(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks equationArgumentPack);
        I_Equation Rebuild(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks equationArgumentPack);
        float GetLow(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks equationArguments);
        float GetHigh(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks equationArguments);
        void AddInvalidationListener(I_DeliveryTool toolManager, I_InvalidationListener listener, InvalidationIdentifier identifier);
        void RemoveInvalidationListener(I_DeliveryTool deliveryTool, I_InvalidationListener listener);
    }
}
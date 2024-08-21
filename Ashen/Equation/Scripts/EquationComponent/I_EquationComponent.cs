using Ashen.DeliverySystem;

namespace Ashen.EquationSystem
{
    public interface I_EquationComponent
    {
        float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments);
        string Representation();
        bool IsOperation();
        bool IsArgumentOperation();
        bool RequiresRebuild();
        I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks extraArguments);
        bool RequiresCaching();
        bool IsCachable();
        bool Cache(I_DeliveryTool source, Equation equation);
        bool InvalidComponent();
    }
}
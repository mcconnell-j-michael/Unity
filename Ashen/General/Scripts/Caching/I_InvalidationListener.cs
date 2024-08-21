using Ashen.DeliverySystem;
using Ashen.EquationSystem;

public interface I_InvalidationListener
{
    void Invalidate(I_DeliveryTool toolManager, InvalidationIdentifier identifier);
}

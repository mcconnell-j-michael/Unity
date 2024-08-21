namespace Ashen.PartySystem
{
    public interface I_PartyResourceChangeListener
    {
        void OnChange(PartyResource resource, int oldValue, int newValue, int oldReservedValue, int newReservedValue);
    }
}
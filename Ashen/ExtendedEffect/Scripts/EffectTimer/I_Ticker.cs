using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    /**
     * Any created Ticker must override this interface
     **/
    public interface I_Ticker
    {
        void Reset();
        void Reset(int? duration, int frequency);
        void Enable(I_Tickable tickable);
        void Disable();
        void Remove();
        float? TimeLeft();
        float? TotalDuration();
        I_Ticker Duplicate();
        void UpdateTime();
    }
}

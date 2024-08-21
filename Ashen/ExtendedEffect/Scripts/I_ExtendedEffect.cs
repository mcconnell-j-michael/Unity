using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashen.DeliverySystem
{
    /**
     * The interface that any implementation of DerivedStatusEffect must implement
     **/
    [HideLabel, BoxGroup("Status Effect")]
    public interface I_ExtendedEffect : I_Tickable
    {
        void Enable();
        void Disable(bool natural);
        void Remove();
        void Trigger(ExtendedEffectTrigger statusTrigger);
        I_DeliveryTool Target();
        void SetTicker(I_Ticker ticker);
    }
}

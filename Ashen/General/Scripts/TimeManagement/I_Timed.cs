using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashen.ToolSystem
{
    /**
     * Every class that needs to be on a timer must extend this class.
     * A good example of this are StatusEffects that need to last
     * for a certain duration and/or must 'tick' every x seconds 
     * while they are applied
     **/
    public interface I_Timed
    {
        void UpdateTime();
        bool IsEnabled();
        void StartTracking();
        void StopTracking();
        bool IsTracked();
    }
}

using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

namespace Ashen.ToolSystem
{
    public interface I_TriggerListener
    {
        void OnTrigger(ExtendedEffectTrigger trigger);
    }
}
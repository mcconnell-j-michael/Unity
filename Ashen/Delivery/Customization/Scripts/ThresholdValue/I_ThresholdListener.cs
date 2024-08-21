using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_ThresholdListener
    {
        void OnThresholdEvent(ThresholdEventValue value);
    }
}
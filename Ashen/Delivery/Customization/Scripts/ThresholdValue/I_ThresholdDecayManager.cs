using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

public interface I_ThresholdDecayManager
{
    void Init(A_ThresholdValue value, I_DeliveryTool deliveryTool);
}

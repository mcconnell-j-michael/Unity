using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

namespace Ashen.DeliverySystem
{
    public interface I_ThresholdValue
    {
        void TakeDamage(int damage);
        void UndoDamage(int damage);
        void ClearDamage();
        void Init(I_DeliveryTool deliveryTool);
        A_ThresholdValue Clone();
    }
}
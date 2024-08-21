using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(ShiftCategory), menuName = "Custom/Enums/" + nameof(ShiftCategory) + "/Type")]
    public class ShiftCategory : A_EnumSO<ShiftCategory, ShiftCategories>
    {
        public bool multiplier;
    }
}
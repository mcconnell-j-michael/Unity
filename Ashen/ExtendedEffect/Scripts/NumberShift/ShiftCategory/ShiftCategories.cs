using Ashen.ToolSystem;
using System.Linq;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(ShiftCategories), menuName = "Custom/Enums/" + nameof(ShiftCategories) + "/Types")]
    public class ShiftCategories : A_EnumList<ShiftCategory, ShiftCategories>
    {
        public ShiftCategory GetDefault()
        {
            return AttributeLimiters.Instance.DEFAULT_ATTRIBUTE_LIMITER.GetShiftCategories().FirstOrDefault();
        }
    }
}
using UnityEngine;
using System.Collections;
using Ashen.ToolSystem;
using Ashen.DeliverySystem;

namespace Ashen.DeliverySystem
{
    public class ShiftableValue : A_Shiftable
    {
        private float baseValue;

        public override float GetBase(I_DeliveryTool toolManager)
        {
            return baseValue;
        }

        public void AddBase(float baseValue)
        {
            this.baseValue += baseValue;
            valid = false;
        }
    }
}
using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class RangeValue : A_DeliveryValue
    {
        [HorizontalGroup(nameof(value), Width = 0.10f)]
        [Title("Zero"), HideLabel]
        [SerializeField]
        private bool zero;
        [HorizontalGroup(nameof(value))]
        [Title("Value"), HideWithoutAutoPopulate, Range(1, 100), DisableIf(nameof(zero))]
        [SerializeField]
        private int value;
        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            if (zero)
            {
                return 0;
            }
            return value;
        }

        public override string Visualize()
        {
            if (zero)
            {
                return "0";
            }
            return value.ToString();
        }

        public RangeValue(SerializationInfo info, StreamingContext context)
        {
            zero = info.GetBoolean(nameof(zero));
            value = info.GetInt32(nameof(value));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(zero), zero);
            info.AddValue(nameof(value), value);
        }
    }
}

using Ashen.EnumSystem;
using Ashen.EquationSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class EquationValue : A_RecalculatedDeliveryValue
    {
        [OdinSerialize, HideLabel]
        private Equation value;

        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return value.Calculate(owner, target, deliveryArguments);
        }

        public override string Visualize()
        {
            return value.ToString();
        }

        protected override void OnRegisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            value.AddInvalidationListener(deliveryTool, listener, new InvalidationIdentifier()
            {
                source = "EquationValue",
                enumKey = enumSO,
                key = enumSO.ToString()
            });
        }

        protected override void OnDeregisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            value.RemoveInvalidationListener(deliveryTool, listener);
        }

        public EquationValue(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            value = (Equation)info.GetValue(nameof(value), typeof(EquationValue));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(value), value);
        }
    }
}

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
    public class ScalingValueBuilder : A_RecalculatedDeliveryValue
    {
        [OdinSerialize, HideLabel]
        private I_Equation equationValue;
        [OdinSerialize, FoldoutGroup("Scaling")]
        private I_DeliveryValue multipleScale;
        [OdinSerialize, FoldoutGroup("Scaling")]
        private I_DeliveryValue flatScale;

        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            float value = equationValue.Calculate(owner, target, deliveryArguments);
            if (multipleScale != null)
            {
                value *= multipleScale.Build(owner, target, deliveryArguments);
            }
            if (flatScale != null)
            {
                value += flatScale.Build(owner, target, deliveryArguments);
            }
            return value;
        }

        public override string Visualize()
        {
            string value = equationValue.ToString();
            if (multipleScale != null)
            {
                value = "((" + value + ") * (" + multipleScale.Visualize() + "))";
            }
            if (flatScale != null)
            {
                value += " + " + flatScale.Visualize();
            }
            return value;
        }

        protected override void OnRegisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            equationValue.AddInvalidationListener(deliveryTool, listener, new InvalidationIdentifier()
            {
                source = "EquationValue",
                enumKey = enumSO,
                key = enumSO.ToString()
            });
            if (multipleScale != null)
            {
                multipleScale.OnRegister(deliveryTool, listener, enumSO);
            }
            if (flatScale != null)
            {
                flatScale.OnRegister(deliveryTool, listener, enumSO);
            }
        }

        protected override void OnDeregisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            equationValue.RemoveInvalidationListener(deliveryTool, listener);
            if (multipleScale != null)
            {
                multipleScale.OnDeregister(deliveryTool, listener, enumSO);
            }
            if (flatScale != null)
            {
                flatScale.OnDeregister(deliveryTool, listener, enumSO);
            }
        }

        public ScalingValueBuilder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            equationValue = StaticUtilities.LoadInterfaceValue<I_Equation>(info, nameof(equationValue));
            multipleScale = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(multipleScale));
            flatScale = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(flatScale));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            StaticUtilities.SaveInterfaceValue(info, nameof(equationValue), equationValue);
            StaticUtilities.SaveInterfaceValue(info, nameof(multipleScale), multipleScale);
            StaticUtilities.SaveInterfaceValue(info, nameof(flatScale), flatScale);
        }
    }
}
using Ashen.EnumSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ScaledValue : A_RecalculatedDeliveryValue
    {
        [OdinSerialize, HideLabel]
        private I_DeliveryValue deliveryValue;
        [OdinSerialize, FoldoutGroup("Scaling")]
        private I_DeliveryValue multipleScale;
        [OdinSerialize, FoldoutGroup("Scaling")]
        private I_DeliveryValue flatScale;

        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            float value = deliveryValue.Build(owner, target, deliveryArguments);
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
            string value = deliveryValue.Visualize();
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
            deliveryValue.OnRegister(deliveryTool, listener, enumSO);
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
            deliveryValue.OnDeregister(deliveryTool, listener, enumSO);
            if (multipleScale != null)
            {
                multipleScale.OnDeregister(deliveryTool, listener, enumSO);
            }
            if (flatScale != null)
            {
                flatScale.OnDeregister(deliveryTool, listener, enumSO);
            }
        }

        public ScaledValue(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            deliveryValue = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(deliveryValue));
            multipleScale = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(multipleScale));
            flatScale = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(flatScale));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            StaticUtilities.SaveInterfaceValue(info, nameof(deliveryValue), deliveryValue);
            StaticUtilities.SaveInterfaceValue(info, nameof(multipleScale), multipleScale);
            StaticUtilities.SaveInterfaceValue(info, nameof(flatScale), flatScale);
        }
    }
}

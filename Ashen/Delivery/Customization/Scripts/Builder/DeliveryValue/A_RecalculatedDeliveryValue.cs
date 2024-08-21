using Ashen.EnumSystem;
using Ashen.ToolSystem;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public abstract class A_RecalculatedDeliveryValue : A_DeliveryValue
    {
        [OdinSerialize]
        private bool recalculateOnChange;

        public override void OnRegister(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            if (recalculateOnChange)
            {
                OnRegisterInternal(deliveryTool, listener, enumSO);
            }
        }

        public override void OnDeregister(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            if (recalculateOnChange)
            {
                OnDeregisterInternal(deliveryTool, listener, enumSO);
            }
        }

        protected abstract void OnRegisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO);
        protected abstract void OnDeregisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO);

        protected A_RecalculatedDeliveryValue(SerializationInfo info, StreamingContext context)
        {
            recalculateOnChange = info.GetBoolean(nameof(recalculateOnChange));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(recalculateOnChange), recalculateOnChange);
        }
    }
}
using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ExtendedEffectTagCondition : I_EffectCondition
    {
        [HideLabel, FoldoutGroup("Condition"), InlineProperty]
        public I_TagConditional conditional;

        public bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return conditional.Check(owner, target, deliveryArguments);
        }

        public string visualize()
        {
            return conditional.visualize();
        }

        public ExtendedEffectTagCondition(SerializationInfo info, StreamingContext context)
        {
            conditional = StaticUtilities.LoadInterfaceValue<I_TagConditional>(info, nameof(conditional));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(conditional), conditional);
        }
    }
}
using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class EffectFloatArgumentValue : A_DeliveryValue
    {
        [SerializeField, HideLabel, EnumSODropdown]
        private EffectFloatArgument argument;

        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            EffectsArgumentPack effectPack = deliveryArguments.GetPack<EffectsArgumentPack>();
            return effectPack.GetFloatScale(argument);
        }

        public override string Visualize()
        {
            return "Argument[" + argument.name + "]";
        }

        public EffectFloatArgumentValue(SerializationInfo info, StreamingContext context)
        {
            argument = (EffectFloatArgument)EffectFloatArguments.Instance[info.GetInt32(nameof(argument))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(argument), (int)argument);
        }
    }
}

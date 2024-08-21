using Sirenix.OdinInspector;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    public class ExtendedEffectArgumentValue : A_DeliveryValue
    {
        [SerializeField, EnumSODropdown, HideLabel]
        private ExtendedEffectArgument argument;

        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ExtendedEffectArgumentsPack extendedEffectArgumentsPack = deliveryArguments.GetPack<ExtendedEffectArgumentsPack>();
            return extendedEffectArgumentsPack.GetFloatArgumentFlat(argument);

        }

        public override string Visualize()
        {
            return "Argument[" + argument.name + "]";
        }

        public ExtendedEffectArgumentValue(SerializationInfo info, StreamingContext context)
        {
            argument = ExtendedEffectArguments.Instance[info.GetInt32(nameof(argument))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(argument), (int)argument);
        }
    }
}

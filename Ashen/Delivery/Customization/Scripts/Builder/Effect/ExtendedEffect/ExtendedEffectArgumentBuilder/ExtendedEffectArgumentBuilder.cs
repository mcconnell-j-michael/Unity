using Ashen.EquationSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ExtendedEffectArgumentBuilder : I_ExtendedEffectArgumentBuilder
    {
        [SerializeField, EnumSODropdown]
        private ExtendedEffectArgument argument;
        [OdinSerialize, HideWithoutAutoPopulate, AutoPopulate(typeof(Equation)), Title(nameof(equation))]
        private I_Equation equation;

        public void FillArguments(ExtendedEffectArgumentFiller filler, I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            int value = (int)equation.Calculate(owner, deliveryArguments);
            filler.SetValue(argument, value);
        }

        public ExtendedEffectArgumentBuilder(SerializationInfo info, StreamingContext context)
        {
            argument = ExtendedEffectArguments.Instance[info.GetInt32(nameof(argument))];
            equation = StaticUtilities.LoadInterfaceValue<I_Equation>(info, nameof(equation));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(argument), (int)argument);
            StaticUtilities.SaveInterfaceValue(info, nameof(equation), equation);
        }
    }
}

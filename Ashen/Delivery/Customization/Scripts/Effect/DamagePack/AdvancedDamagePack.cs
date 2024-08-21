using Ashen.EquationSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * A DamagePack is an Effect that contains a DynamicDamageType and a DynamicNumber
     * The DynamicDamageType is used to determine the types of damage to do and how to 
     * distribute the results of the DynamicNumber
     **/
    [InlineProperty]
    [Serializable]
    public class AdvancedDamagePack : I_Effect
    {
        [HorizontalGroup(nameof(AdvancedDamagePack)), OdinSerialize, InlineProperty, HideReferenceObjectPicker, LabelText("O"), LabelWidth(20)]
        public I_DynamicDamageType DamageTypes;
        [HorizontalGroup(nameof(AdvancedDamagePack), width: 0.5f), OdinSerialize, HideLabel]
        public I_Equation equation;

        private float equationValue;

        public AdvancedDamagePack() { }
        public AdvancedDamagePack(I_DynamicDamageType damageType, I_Equation dynamicNumber)
        {
            DamageTypes = damageType;
            equation = dynamicNumber;
        }

        public int GetAmount(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            if (equation != null)
            {
                return (int)equation.Calculate(owner, target, deliveryArguments);
            }
            return (int)equationValue;
        }

        public void Apply(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            int total = GetAmount(dOwner, dTarget, deliveryArguments);
            ToolManager target = (dTarget as DeliveryTool).toolManager;
            List<DamageRatio> damageTypeList = DamageTypes.GetDamageTypes(target);
            DamageResult deliveryResult = targetDeliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            foreach (DamageRatio pair in damageTypeList)
            {
                DamageType damageType = pair.damageType;
                float percent = pair.ratio;
                int portion = (int)(total * percent);
                deliveryResult.AddDamage(damageType, portion);
                if (portion != 0)
                {
                    targetDeliveryResult.empty = false;
                }
            }
        }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new AdvancedDamagePack
            {
                DamageTypes = DamageTypes,
                equationValue = equation.Calculate(owner, target, deliveryArguments)
            };
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Deal [" + equation.ToString() + "] OF " + DamageTypes.ToString();
            return vis;
        }

        public AdvancedDamagePack(SerializationInfo info, StreamingContext context)
        {
            DamageTypes = StaticUtilities.LoadInterfaceValue<I_DynamicDamageType>(info, nameof(DamageTypes));
            equation = StaticUtilities.LoadInterfaceValue<I_Equation>(info, nameof(equation));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(DamageTypes), DamageTypes);
            StaticUtilities.SaveInterfaceValue(info, nameof(equation), equation);
        }
    }
}
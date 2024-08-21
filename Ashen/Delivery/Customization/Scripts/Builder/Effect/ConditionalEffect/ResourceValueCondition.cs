using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ResourceValueCondition : I_EffectCondition
    {
        public CompareType type;
        public ResourceValue resourceValue;
        public Comparable comparable;
        public I_DeliveryValue deliveryValue;

        [HideIf(nameof(comparable), Comparable.EQ)]
        public bool equal;

        public bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ResourceValueTool resourceValueTool = ((DeliveryTool)target).toolManager.Get<ResourceValueTool>();
            ThresholdEventValue value = resourceValueTool.GetValue(resourceValue);
            int thresholdValue = (type == CompareType.PERCENTAGE) ? ((int)(value.currentValue / (float)value.maxValue)) : value.currentValue;
            int equationValue = (int)deliveryValue.Build(owner, target, deliveryArguments);

            switch (comparable)
            {
                case Comparable.EQ:
                    return equationValue == thresholdValue;
                case Comparable.GT:
                    return equal ? thresholdValue >= equationValue : thresholdValue > equationValue;
                case Comparable.LT:
                    return equal ? thresholdValue <= equationValue : thresholdValue < equationValue;
            }

            return false;
        }

        public string visualize()
        {
            string compStr = "";
            switch (comparable)
            {
                case Comparable.EQ:
                    compStr = "=";
                    break;
                case Comparable.GT:
                    compStr = ">" + (equal ? "=" : "");
                    break;
                case Comparable.LT:
                    compStr = "<" + (equal ? "=" : "");
                    break;
            }
            return resourceValue.name + " " + compStr + " " + deliveryValue.ToString();
        }
        public ResourceValueCondition(SerializationInfo info, StreamingContext context)
        {
            type = (CompareType)info.GetValue(nameof(type), typeof(CompareType));
            resourceValue = ResourceValues.Instance[info.GetInt32(nameof(resourceValue))];
            comparable = (Comparable)info.GetValue(nameof(comparable), typeof(Comparable));
            deliveryValue = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(deliveryValue));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(type), type);
            info.AddValue(nameof(resourceValue), (int)resourceValue);
            info.AddValue(nameof(comparable), comparable);
            StaticUtilities.SaveInterfaceValue(info, nameof(deliveryValue), deliveryValue);
        }

    }

    [Serializable]
    public enum Comparable
    {
        GT, LT, EQ
    }

    [Serializable]
    public enum CompareType
    {
        PERCENTAGE, VALUE
    }
}
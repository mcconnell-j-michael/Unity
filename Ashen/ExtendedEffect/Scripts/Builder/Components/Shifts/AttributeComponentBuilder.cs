using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class AttributeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private DerivedAttribute attributeType = default;
        [OdinSerialize]
        private ShiftCategory shiftCategory = default;
        [OdinSerialize]
        private I_DeliveryValue deliveryValue;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new AttributeComponent(attributeType, shiftCategory, new SimpleValue(deliveryValue.Build(owner, target, deliveryArgument)), 1);
        }

        public AttributeComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            attributeType = DerivedAttributes.Instance[info.GetInt32(nameof(attributeType))];
            shiftCategory = ShiftCategories.Instance[info.GetInt32(nameof(shiftCategory))];
            deliveryValue = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(deliveryValue));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(attributeType), (int)attributeType);
            info.AddValue(nameof(shiftCategory), (int)shiftCategory);
            StaticUtilities.SaveInterfaceValue(info, nameof(deliveryValue), deliveryValue);
        }
    }
}
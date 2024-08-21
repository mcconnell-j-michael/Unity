using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ResistanceComponentBuilder : I_ComponentBuilder
    {

        [HorizontalGroup(nameof(ResistanceComponent))]

        [VerticalGroup(nameof(ResistanceComponent) + "/" + nameof(ResistanceType))]
        [OdinSerialize, EnumSODropdown, HideLabel, Title("Resistance Type")]
        private DamageType ResistanceType = default;
        [VerticalGroup(nameof(ResistanceComponent) + "/" + nameof(shiftCategory))]
        [OdinSerialize, HideLabel, Title("Shift Category"), EnumSODropdown]
        private ShiftCategory shiftCategory = default;
        [VerticalGroup(nameof(I_DeliveryValue))]
        [OdinSerialize, HideLabel, Title("Equation")]
        private I_DeliveryValue deliveryValue;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgument)
        {
            return new ResistanceComponent(ResistanceType, shiftCategory, (int)deliveryValue.Build(owner, target, deliveryArgument), 1);
        }

        public ResistanceComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            ResistanceType = DamageTypes.Instance[info.GetInt32(nameof(ResistanceType))];
            shiftCategory = ShiftCategories.Instance[info.GetInt32(nameof(ShiftCategory))];
            deliveryValue = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(deliveryValue));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ResistanceType), (int)ResistanceType);
            info.AddValue(nameof(shiftCategory), shiftCategory);
            StaticUtilities.SaveInterfaceValue(info, nameof(deliveryValue), deliveryValue);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ComponentListComponent : A_SimpleComponent
    {
        private List<I_ExtendedEffectComponent> subComponents;
        public ComponentListComponent(List<I_ExtendedEffectComponent> subComponents)
        {
            this.subComponents = subComponents;
        }

        public override List<I_ExtendedEffectComponent> BreakDown()
        {
            return subComponents;
        }

        public ComponentListComponent(SerializationInfo info, StreamingContext context)
        {
            subComponents = StaticUtilities.LoadList(info, nameof(subComponents), (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_ExtendedEffectComponent>(info, name);
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(subComponents), subComponents, (string name, I_ExtendedEffectComponent component) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, component);
            });
        }
    }
}

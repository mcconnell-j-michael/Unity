using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ComponentListComponentBuilder : I_ComponentBuilder
    {
        [ListDrawerSettings(ShowFoldout = false), InlineProperty, AutoPopulate]
        public List<I_ComponentBuilder> baseStatusEffects;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            List<I_ExtendedEffectComponent> components = new List<I_ExtendedEffectComponent>();
            if (baseStatusEffects != null)
            {
                foreach (I_ComponentBuilder effect in baseStatusEffects)
                {
                    components.Add(effect.Build(owner, target, deliveryArguments));
                }
            }
            return new ComponentListComponent(components);
        }

        public ComponentListComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            baseStatusEffects = StaticUtilities.LoadList(info, nameof(baseStatusEffects), (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_ComponentBuilder>(info, name);
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(baseStatusEffects), baseStatusEffects, (string name, I_ComponentBuilder componentBuilder) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, componentBuilder);
            });
        }
    }
}

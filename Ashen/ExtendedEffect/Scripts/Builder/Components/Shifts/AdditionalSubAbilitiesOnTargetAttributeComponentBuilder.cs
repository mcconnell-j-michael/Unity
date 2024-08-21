using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.ExtendedEffectSystem
{
    public class AdditionalSubAbilitiesOnTargetAttributeComponentBuilder : I_ComponentBuilder
    {
        [OdinSerialize]
        private TargetAttribute attribute = default;
        [OdinSerialize]
        private int priority = default;
        [BoxGroup(nameof(subAbilities)), OdinSerialize, HideLabel]
        private List<SubAbilityBuilder> subAbilities = default;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new AdditionalSubAbilitiesOnTargetAttributeComponent(attribute, null, subAbilities, priority);
        }

        public AdditionalSubAbilitiesOnTargetAttributeComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            attribute = TargetAttributes.Instance[info.GetInt32(nameof(attribute))];
            priority = info.GetInt32(nameof(priority));
            subAbilities = StaticUtilities.LoadList(info, nameof(subAbilities), (string name) =>
            {
                return (SubAbilityBuilder)info.GetValue(name, typeof(SubAbilityBuilder));
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(attribute), (int)attribute);
            info.AddValue(nameof(priority), priority);
            StaticUtilities.SaveList(info, nameof(subAbilities), subAbilities, (string name, SubAbilityBuilder subAbility) =>
            {
                info.AddValue(name, subAbility);
            });
        }
    }
}

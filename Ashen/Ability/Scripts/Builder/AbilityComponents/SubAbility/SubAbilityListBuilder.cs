using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class SubAbilityListBuilder : I_AbilityBuilder
    {
        public TargetAttribute targetAttribute;

        public List<SubAbilityBuilder> subAbilities;

        public I_AbilityProcessor Build(Ability ability)
        {
            SubAbilityProcessor processor = new SubAbilityProcessor(ability, targetAttribute);

            if (subAbilities != null)
            {
                foreach (SubAbilityBuilder builder in subAbilities)
                {
                    processor.AddAbilityAction(builder.Build(ability));
                }
            }

            return processor;
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new SubAbilityListBuilder();
        }

        public string GetTabName()
        {
            return "SubAbility";
        }
    }
}
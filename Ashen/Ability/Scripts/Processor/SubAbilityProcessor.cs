using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class SubAbilityProcessor : A_AbilityProcessor
    {
        private Ability originalAbility;
        private List<AbilityAction> subAbilities;
        private TargetAttribute targetAttribute;

        public SubAbilityProcessor(Ability originalAbility, TargetAttribute targetAttribute)
        {
            this.originalAbility = originalAbility;
            subAbilities = new();
            this.targetAttribute = targetAttribute;
        }

        public void AddAbilityAction(AbilityAction action)
        {
            subAbilities.Add(action);
        }

        public void ResetAbilityActions()
        {
            subAbilities.Clear();
        }

        public List<AbilityAction> GetAbilityActions(ToolManager toolManager)
        {
            List<AbilityAction> actions = new();
            actions.AddRange(subAbilities);
            if (toolManager != null && targetAttribute != null)
            {
                ShiftableAdditionalSubAbilitiesTool shiftableAdditionalSubAbilitiesTool = toolManager.Get<ShiftableAdditionalSubAbilitiesTool>();
                foreach (SubAbilityBuilder subAbilityBuilder in shiftableAdditionalSubAbilitiesTool.Get(targetAttribute))
                {
                    actions.Add(subAbilityBuilder.Build(originalAbility));
                }
            }
            return actions;
        }
    }
}
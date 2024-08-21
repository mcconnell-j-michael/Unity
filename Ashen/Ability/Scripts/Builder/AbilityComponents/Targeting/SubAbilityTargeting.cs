using Sirenix.OdinInspector;
using System;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class SubAbilityTargeting : I_SubAbilityBuilder
    {
        [EnumToggleButtons]
        public SubAbilityRelativeTarget relativeTarget;

        [EnumToggleButtons, Title("Who to target"), HideLabel]
        [ShowIf("@" + nameof(relativeTarget) + " == " + nameof(SubAbilityRelativeTarget) + "." + nameof(SubAbilityRelativeTarget.Random))]
        public TargetParty targetParty;

        public I_AbilityProcessor Build(Ability ability)
        {
            SubAbilityTargetingProcessor processor = new SubAbilityTargetingProcessor();

            TargetingProcessor parentProcessor = ability.abilityAction.Get<TargetingProcessor>();
            processor.parentProcessor = parentProcessor;

            processor.relativeTarget = relativeTarget;
            processor.targetParty = targetParty;

            return new TargetingProcessor(processor);
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new SubAbilityTargeting();
        }

        public string GetTabName()
        {
            return "Targeting";
        }
    }
}
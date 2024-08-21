using Ashen.AbilitySystem;
using Sirenix.OdinInspector;

namespace Ashen.SkillTree
{
    public class TargetingSubAbilityOverrideComponent : I_SubAbilityOverrideComponent
    {
        [EnumToggleButtons]
        public SubAbilityRelativeTarget relativeTarget;

        [EnumToggleButtons, Title("Who to target"), HideLabel]
        [ShowIf("@" + nameof(relativeTarget) + " == " + nameof(SubAbilityRelativeTarget) + "." + nameof(SubAbilityRelativeTarget.Random))]
        public TargetParty targetParty;

        public void Override(AbilityAction abilityAction)
        {
            TargetingProcessor processor = abilityAction.Get<TargetingProcessor>();
            SubAbilityTargetingProcessor targetingProcessor = processor.GetTargetingValue() as SubAbilityTargetingProcessor;
            if (targetingProcessor == null)
            {
                return;
            }
            targetingProcessor.targetParty = targetParty;
            targetingProcessor.relativeTarget = relativeTarget;
        }
    }
}
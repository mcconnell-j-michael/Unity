using Ashen.AbilitySystem;
using UnityEngine;

namespace Ashen.SkillTree
{
    public class AnimationAbilityOverrideComponent : I_AbilityOverrideComponent, I_SubAbilityOverrideComponent
    {
        public GameObject animation;
        public void Override(AbilityAction abilityAction)
        {
            AbilityAnimationProcessor animationProcessor = abilityAction.Get<AbilityAnimationProcessor>();
            animationProcessor.animation = animation;
        }
    }
}
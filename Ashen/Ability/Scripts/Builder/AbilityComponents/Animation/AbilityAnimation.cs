using Ashen.ItemSystem;
using System;
using UnityEngine;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class AbilityAnimation : I_AbilityBuilder, I_SubAbilityBuilder, I_ItemBuilder
    {
        public GameObject animation;

        public I_AbilityProcessor Build(Ability ability)
        {
            AbilityAnimationProcessor processor = new AbilityAnimationProcessor
            {
                animation = animation
            };
            return processor;
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new AbilityAnimation();
        }

        public string GetTabName()
        {
            return "Animation";
        }
    }
}
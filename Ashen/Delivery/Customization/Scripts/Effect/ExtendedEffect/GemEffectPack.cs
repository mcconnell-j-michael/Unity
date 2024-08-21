using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    public class GemEffectPack : I_Effect
    {
        [OdinSerialize]
        public I_ExtendedEffectBuilder gemEffect = default;
        [OdinSerialize]
        private GemTickerPack gemTickerPack = default;
        [OdinSerialize]
        public Description description;
        [NonSerialized, HideInInspector]
        public string key;

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetResultPack, DeliveryArgumentPacks deliveryArguments)
        {
            I_ExtendedEffect statusEffect = gemEffect.Build(owner, target, deliveryArguments);
            if (gemTickerPack != null)
            {
                statusEffect.SetTicker(gemTickerPack.Build(owner, target, deliveryArguments));
            }
            StatusEffectResult deliveryResult = targetResultPack.GetResult<StatusEffectResult>(DeliveryResultTypes.Instance.STATUS_EFFECT_RESULT_TYPE);
            deliveryResult.AppliedStatusEffects.Add(statusEffect);
            targetResultPack.empty = false;
        }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return this;
        }

        public string visualize(int depth)
        { 
            throw new NotImplementedException();
        }
    }
}

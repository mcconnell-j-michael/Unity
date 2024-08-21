using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public class StatusEffectResult : A_DeliveryResult
    {
        public List<I_ExtendedEffect> AppliedStatusEffects { get; private set; }

        public StatusEffectResult()
        {
            AppliedStatusEffects = new List<I_ExtendedEffect>();
        }

        public override void Clear()
        {
            AppliedStatusEffects.Clear();
        }

        public override void Calculate(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
        }

        public override void Deliver(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            foreach (ExtendedEffect dse in AppliedStatusEffects)
            {
                dse.Enable();
            }
        }

        public override A_DeliveryResult Clone()
        {
            return new StatusEffectResult();
        }
    }
}
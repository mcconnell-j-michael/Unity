using UnityEngine;
using System.Collections;
using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public class CriticalResult : A_DeliveryResult
    {
        private bool critical;
        private bool calculated;
        public DerivedAttribute criticalChance;
        public DerivedAttribute criticalMultiplier;

        public CriticalResult() { }

        public CriticalResult(DerivedAttribute criticalChance, DerivedAttribute criticalMultiplier)
        {
            this.criticalChance = criticalChance;
            this.criticalMultiplier = criticalMultiplier;
            critical = false;
            calculated = false;
        }

        public bool IsCritical(I_DeliveryTool owner, I_DeliveryTool target)
        {
            if (!calculated)
            {
                InnerCalculate(owner, target);
            }
            return critical;
        }

        private void InnerCalculate(I_DeliveryTool owner, I_DeliveryTool target)
        {
            //DeliveryTool deliveryOwner = owner as DeliveryTool;
            //float critPercentage = criticalChance.Get(owner);
            //int randomRoll = Random.Range(0, 100);
            //if (randomRoll < critPercentage)
            //{
            //    critical = true;
            //}
            //else
            //{
            //    critical = false;
            //}
            //calculated = true;
        }

        public override void Calculate(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            IsCritical(owner, target);
        }

        public override void Clear()
        {
            critical = false;
            calculated = false;
        }

        public override void Deliver(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {}

        public override A_DeliveryResult Clone()
        {
            return new CriticalResult(criticalChance, criticalMultiplier);
        }
    }
}
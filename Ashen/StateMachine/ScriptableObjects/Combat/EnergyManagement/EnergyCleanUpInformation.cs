using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    [Serializable]
    public class EnergyCleanUpInformation
    {
        [SerializeField]
        private ResourceValue value;
        [SerializeField]
        private DerivedAttribute midwayValue;
        [SerializeField]
        private DamageType energyDamageType;

        public ResourceValue Value { get { return value; } }
        public DerivedAttribute MidwayValue { get { return midwayValue; } }
        public DamageType EnergyDamageType { get { return energyDamageType; } }
    }
}
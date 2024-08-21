using Ashen.ToolSystem;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class CombatInfusion : A_EnumSO<CombatInfusion, CombatInfusions>
    {
        [SerializeField]
        private DerivedAttribute infusionEffectResistance;
        public DerivedAttribute InfusionEffectResist { get { return infusionEffectResistance; } }
        [SerializeField]
        private DerivedAttribute infusionEffectResistanceMult;
        public DerivedAttribute InfusionEffectResistanceMult { get { return infusionEffectResistanceMult; } }
        [SerializeField]
        private DerivedAttribute infusionLevel;
        public DerivedAttribute InfusionLevel { get { return infusionLevel; } }
        [SerializeField]
        private DerivedAttribute infusionMaxLevel;
        public DerivedAttribute InfusionMaxLevel { get { return infusionMaxLevel; } }
        [SerializeField]
        private DerivedAttribute saturationBaseLimit;
        public DerivedAttribute SaturationBaseLimit { get { return saturationBaseLimit; } }
        [SerializeField]
        private DerivedAttribute saturationBonusLimit;
        public DerivedAttribute SaturationBonusLimit { get { return saturationBonusLimit; } }
        [SerializeField]
        private DerivedAttribute saturationLimit;
        public DerivedAttribute SaturationLimit { get { return saturationLimit; } }
        [SerializeField]
        private DerivedAttribute saturationMax;
        public DerivedAttribute SaturationMax { get { return saturationMax; } }
        [SerializeField]
        private ResourceValue currentSaturationValue;
        public ResourceValue CurrentSaturationValue { get { return currentSaturationValue; } }
    }
}
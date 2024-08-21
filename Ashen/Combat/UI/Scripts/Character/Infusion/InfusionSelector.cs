using Ashen.AbilitySystem;
using JoshH.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public class InfusionSelector : MonoBehaviour
    {
        [SerializeField]
        private Image defaultBackground;
        [SerializeField]
        private UIGradient selectedGradient;

        [SerializeField]
        private Color canBeSelectedTop;
        [SerializeField]
        private Color canBeSelectedBottom;
        [SerializeField]
        private Color cannotBeSelectedTop;
        [SerializeField]
        private Color cannotBeSelectedBottom;

        [SerializeField]
        private CombatInfusion currentBuildUp;
        public CombatInfusion CurrentBuildUp { get { return currentBuildUp; } }
        [SerializeField]
        private DerivedAttribute currentInfusionLevel;
        public DerivedAttribute CurrentInfusionLevel { get { return currentInfusionLevel; } }
        [SerializeField]
        private DerivedAttribute maxInfustionLevel;
        public DerivedAttribute MaxInfustionLevel { get { return maxInfustionLevel; } }
        [SerializeField]
        private DerivedAttribute minimumRequiredBuildUp;
        public DerivedAttribute MinimumRequiredBuildUp { get { return minimumRequiredBuildUp; } }
        [SerializeField]
        private AbilitySO infusionEffect;
        public AbilitySO InfusionEffect { get { return infusionEffect; } }
        [SerializeField]
        private AbilitySO diffusionEffect;
        public AbilitySO DiffusionEffect { get { return diffusionEffect; } }

        private bool canBeSubmitted;
        public bool CanBeSubmitted { get { return canBeSubmitted; } }

        public void StartSelection(bool canBeSelected)
        {
            selectedGradient.LinearColor1 = canBeSelected ? canBeSelectedTop : cannotBeSelectedTop;
            selectedGradient.LinearColor2 = canBeSelected ? canBeSelectedBottom : cannotBeSelectedBottom;
            canBeSubmitted = canBeSelected;
        }

        public void StopSelection()
        {
            selectedGradient.enabled = false;
        }

        public void Select()
        {
            selectedGradient.enabled = true;
        }

        public void Deselect()
        {
            selectedGradient.enabled = false;
        }
    }
}

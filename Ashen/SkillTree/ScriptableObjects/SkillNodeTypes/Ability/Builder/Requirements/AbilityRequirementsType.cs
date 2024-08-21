using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ashen.SkillTree
{
    public class AbilityRequirementsType : MonoBehaviour
    {
        [HideLabel, EnumToggleButtons]
        public AbilityRequirementsTypeInspector type;
        [ShowIf(nameof(type), AbilityRequirementsTypeInspector.Override), Title("Requirement"), HideLabel]
        public I_AbilityRequirement requirement;
    }

    public enum AbilityRequirementsTypeInspector
    {
        Remove, Override
    }
}
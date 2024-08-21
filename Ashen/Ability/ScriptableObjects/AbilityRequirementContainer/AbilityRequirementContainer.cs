using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class AbilityRequirementContainer : SerializedScriptableObject
    {
        [OdinSerialize]
        public List<I_AbilityRequirement> abilityRequirements;
    }
}
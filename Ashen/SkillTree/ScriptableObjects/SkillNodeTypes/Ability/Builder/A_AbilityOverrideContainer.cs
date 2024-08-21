using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    public class A_AbilityOverrideContainer<T> where T : I_BaseAbilityOverrideComponent
    {
        [OdinSerialize, NonSerialized]
        [OnValueChanged(nameof(OnProcessorBuildersChanged))]
        public List<T> abilityOverrides;

        public void OnProcessorBuildersChanged()
        {
            if (abilityOverrides == null)
            {
                return;
            }
            Dictionary<Type, T> typeMap = new();
            for (int x = 0; x < abilityOverrides.Count; x++)
            {
                Type type = abilityOverrides[x].GetType();
                if (typeMap.TryGetValue(type, out _))
                {
                    abilityOverrides.RemoveAt(x);
                    x--;
                }
                else
                {
                    typeMap[type] = abilityOverrides[x];
                }
            }
        }

        public void Override(AbilityAction abilityAction)
        {
            foreach (T overrideComponent in abilityOverrides)
            {
                overrideComponent.Override(abilityAction);
            }
        }
    }
}
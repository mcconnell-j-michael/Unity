using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.AbilitySystem
{
    public class AbilityHolder
    {
        [ShowInInspector]
        private List<Ability> abilities;

        private Dictionary<string, Ability> identifierToAbility;

        public void Initialize()
        {
            abilities = new List<Ability>();
            identifierToAbility = new Dictionary<string, Ability>();
        }

        public IEnumerable<Ability> GetAbilities()
        {
            foreach (Ability ability in abilities)
            {
                yield return ability;
            }
        }

        public int GetCount()
        {
            return abilities.Count;
        }

        public void GrantAbility(string key, Ability ability)
        {
            if (identifierToAbility.TryGetValue(key, out Ability foundAbility))
            {
                abilities.Remove(foundAbility);
                identifierToAbility.Remove(key);
            }
            identifierToAbility.Add(key, ability);
            abilities.Add(ability);
        }

        public void RevokeAbility(string key)
        {
            if (identifierToAbility.TryGetValue(key, out Ability foundAbility))
            {
                abilities.Remove(foundAbility);
                identifierToAbility.Remove(key);
            }
        }

        private List<AbilityTag> emptyTags = new();
        public List<AbilityTag> GetAbilityTags(string key, ToolManager toolManager)
        {
            if (identifierToAbility.TryGetValue(key, out Ability foundAbility))
            {
                return foundAbility.abilityAction.Get<TargetingProcessor>().GetAbilityTags(toolManager);
            }
            return emptyTags;
        }

        public Ability GetRandomAbility()
        {
            int random = Random.Range(0, abilities.Count);
            return abilities[random];
        }
    }
}
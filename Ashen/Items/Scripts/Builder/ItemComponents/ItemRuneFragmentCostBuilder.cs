using Ashen.AbilitySystem;
using Ashen.PartySystem;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Ashen.ItemSystem
{
    [Serializable]
    public class ItemRuneFragmentCostBuilder : I_ItemBuilder
    {
        [OdinSerialize]
        public Dictionary<PartyResource, int> costMap;

        public I_AbilityProcessor Build(Ability ability)
        {
            ItemRuneFragmentCostProcessor processor = new ItemRuneFragmentCostProcessor();
            processor.costMap = new int[PartyResources.Count];
            if (costMap != null)
            {
                foreach (KeyValuePair<PartyResource, int> pair in costMap)
                {
                    processor.costMap[(int)pair.Key] = pair.Value;
                }
            }
            return processor;
        }

        public I_BaseAbilityBuilder CloneBuilder()
        {
            return new ItemRuneFragmentCostBuilder();
        }

        public string GetTabName()
        {
            return "Cost";
        }
    }
}
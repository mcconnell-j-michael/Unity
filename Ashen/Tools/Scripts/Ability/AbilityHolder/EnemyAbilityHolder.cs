using Ashen.AbilitySystem;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.ToolSystem
{
    public class EnemyAbilityHolder : A_EnumeratedTool<EnemyAbilityHolder>
    {
        public List<AbilitySO> abilities;

        public AbilitySO GetRandomAbility()
        {
            int random = Random.Range(0, abilities.Count);
            return abilities[random];
        }
    }
}
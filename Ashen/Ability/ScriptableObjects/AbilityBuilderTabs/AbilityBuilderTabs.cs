using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class AbilityBuilderTabs : SingletonScriptableObject<AbilityBuilderTabs>
    {
        public List<I_BaseAbilityBuilder> builderOrder;
    }
}
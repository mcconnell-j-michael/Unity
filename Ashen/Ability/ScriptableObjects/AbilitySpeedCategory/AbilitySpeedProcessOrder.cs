using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class AbilitySpeedProcessOrder : SingletonScriptableObject<AbilitySpeedProcessOrder>
    {
        public List<AbilitySpeedCategory> speedCategoryOrder;
    }
}
using System.Collections.Generic;
using Ashen.DeliverySystem;

namespace Ashen.ToolSystem
{
    public struct WeaponDamageTypeChange
    {
        public int priority;
        public string source;
        public List<DamageType> damageTypes;
    }
}
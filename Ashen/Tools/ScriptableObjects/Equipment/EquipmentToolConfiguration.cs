using Ashen.DeliverySystem;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class EquipmentToolConfiguration : A_Configuration<EquipmentTool, EquipmentToolConfiguration>
    {
        [OdinSerialize, EnumSODropdown]
        private List<DamageType> baseDamageTypes;

        public List<DamageType> BaseDamageTypes
        {
            get
            {
                if (baseDamageTypes == null)
                {
                    if (GetDefault() == this)
                    {
                        return new List<DamageType>();
                    }
                    return GetDefault().BaseDamageTypes;
                }
                return baseDamageTypes;
            }
        }
    }
}
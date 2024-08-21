using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * The StrongestDamageType will pick out the potentially most effective
     * DamageType out of it's list of DamageTypes
     **/
    public class StrongestDamageType : I_DynamicDamageType
    {
        [OdinSerialize, AutoPopulate, ListDrawerSettings(AlwaysAddDefaultValue = true), EnumSODropdown]
        public List<DamageType> damageTypes;

        private List<DamageRatio> ratios;

        public StrongestDamageType() { }
        public StrongestDamageType(List<DamageType> damageTypes)
        {
            this.damageTypes = damageTypes;
        }

        public List<DamageRatio> GetDamageTypes(ToolManager target)
        {
            ResistanceTool resistanceTool = target.Get<ResistanceTool>();
            if (!resistanceTool)
            {
                return new List<DamageRatio>();
            }
            List<DamageRatio> damages = new List<DamageRatio>();
            int least = 0;
            DamageType leastDamageType = damageTypes[0];
            bool initial = true;
            foreach (DamageType damageType in damageTypes)
            {
                int currentResistance = resistanceTool.GetResistance(damageType);
                if (initial)
                {
                    leastDamageType = damageType;
                    least = currentResistance;
                    initial = false;
                }
                else if (currentResistance < least)
                {
                    leastDamageType = damageType;
                    least = currentResistance;
                }
            }
            if (ratios == null)
            {
                ratios = new List<DamageRatio>();
                ratios.Add(new DamageRatio());
            }
            DamageRatio ratio = ratios[0];
            ratio.damageType = leastDamageType;
            ratio.ratio = 1f;
            damages.Add(ratio);
            return damages;
        }
    }
}

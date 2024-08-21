using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * The SharedRatio is used to simply return back a list of DamageRatios
     **/
    public class SharedRatio : I_DynamicDamageType
    {
        [OdinSerialize, ListDrawerSettings(AlwaysAddDefaultValue = true), LabelText("@" + nameof(GetListName) + "()")]
        private List<DamageRatio> damageRatios;

        private string GetListName()
        {
            if (damageRatios == null || damageRatios.Count == 0)
            {
                return "empty";
            }
            string toReturn = "";
            foreach (DamageRatio damageRatio in damageRatios)
            {
                if (damageRatio == null)
                {
                    continue;
                }
                if (toReturn != "")
                {
                    toReturn += ",";
                }
                if (damageRatio.damageType == null)
                {
                    toReturn += "null";
                }
                else
                {
                    toReturn += damageRatio.damageType.name;
                }
                toReturn += ":" + Math.Round(damageRatio.ratio, 1);
            }
            return toReturn;
        }

        public SharedRatio() { }
        public SharedRatio(List<DamageRatio> damageRatios)
        {
            if (damageRatios == null)
            {
                this.damageRatios = new List<DamageRatio>();
            }
            else
            {
                this.damageRatios = damageRatios;
            }
        }

        public List<DamageRatio> GetDamageTypes(ToolManager target)
        {
            return damageRatios;
        }
    }
}

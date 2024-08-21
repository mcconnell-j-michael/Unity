using Ashen.DeliverySystem;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public struct DamageEvent
    {
        public Tuple<DamageType, int>[] damages;
        public int Total
        {
            get
            {
                int total = 0;
                if (damages == null)
                {
                    return total;
                }
                foreach (Tuple<DamageType, int> damage in damages)
                {
                    if (damage != null)
                    {
                        total += damage.Item2;
                    }
                }
                return total;
            }
        }
        public int GetTotal(List<DamageType> damageTypes)
        {
            int total = 0;
            if (damages == null)
            {
                return total;
            }
            foreach (DamageType damageType in damageTypes)
            {
                if (damages[(int)damageType] == null)
                {
                    continue;
                }
                total += damages[(int)damageType].Item2;
            }
            return total;
        }
        public DamageHitType hitType;
    }
}
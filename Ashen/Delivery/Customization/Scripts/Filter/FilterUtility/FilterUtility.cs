using System.Collections.Generic;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * This class is used to consolidate logic that might be shared between two or more filters
     **/
    public class FilterUtility
    {
        public static int ReduceDamage(List<DamageType> damageTypes, DeliveryResultPack deliveryResult, int reduceBy, int reduceMax, bool reduceFromEach)
        {
            bool reduceAll = reduceBy == -1;
            bool noMax = reduceMax == -1;
            DamageResult damageResult = deliveryResult.GetResult<DamageResult>(DeliveryResultTypes.Instance.DAMAGE_RESULT_TYPE);
            if (reduceAll && noMax)
            {
                ReduceAllDamage(damageTypes, damageResult);
            }
            int totalDamageToReduce = reduceBy;
            if (reduceAll)
            {
                totalDamageToReduce = reduceMax;
            }
            if (!noMax && !reduceAll)
            {
                totalDamageToReduce = Mathf.Min(reduceMax, reduceBy);
            }
            int totalDamageReduced = 0;
            foreach (DamageType damageTypeEnum in damageTypes)
            {
                if (totalDamageToReduce < 1)
                {
                    break;
                }
                int toReduce = Mathf.Min(damageResult.GetDamage(damageTypeEnum), totalDamageToReduce);
                toReduce = Mathf.Max(0, toReduce);
                damageResult.AddDamage(damageTypeEnum, -toReduce);
                if (!reduceFromEach)
                {
                    totalDamageToReduce -= toReduce;
                }
                totalDamageReduced += toReduce;
            }
            return totalDamageReduced;
        }

        public static int ReduceAllDamage(List<DamageType> damageTypes, DamageResult deliveryResult)
        {
            int total = 0;
            foreach (DamageType damageType in damageTypes)
            {
                if (deliveryResult.GetDamage(damageType) > 0)
                {
                    total += deliveryResult.GetDamage(damageType);
                    deliveryResult.ResetDamage(damageType);
                }
            }
            return total;
        }
    }
}
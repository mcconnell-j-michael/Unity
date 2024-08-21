using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class DamageResult : A_DeliveryResult
    {
        public bool combineDamageCalculation;
        [ShowIf(nameof(combineDamageCalculation), true)]
        public DamageType combineInto;

        private bool[] enabledDamageTypes;
        private int[] damageDone;
        private int totalDamage;
        public bool Critical { get; set; }

        public DamageResult()
        {
            enabledDamageTypes = new bool[DamageTypes.Count];
            damageDone = new int[DamageTypes.Count];
            Critical = false;
            totalDamage = 0;
        }

        public int GetDamage(DamageType damageType)
        {
            return damageDone[(int)damageType];
        }

        public void EnableDamageType(DamageType damageType)
        {
            enabledDamageTypes[(int)damageType] = true;
        }

        public void AddDamage(DamageType damageType, int amount)
        {
            damageDone[(int)damageType] += amount;
            totalDamage += amount;
            enabledDamageTypes[(int)damageType] = true;
        }

        public void ResetDamage(DamageType damageType)
        {
            totalDamage -= damageDone[(int)damageType];
            damageDone[(int)damageType] = 0;

            enabledDamageTypes[(int)damageType] = false;
        }

        public override void Clear()
        {
            for (int x = 0; x < damageDone.Length; x++)
            {
                damageDone[x] = 0;
                enabledDamageTypes[x] = false;
            }
            totalDamage = 0;
            Critical = false;
        }

        public override void Calculate(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryArgumentPacks deliveryArguments)
        {
            ToolManager target = (dTarget as DeliveryTool).toolManager;
            if (!combineDamageCalculation)
            {
                DamageTool damageTool = target.Get<DamageTool>();
                for (int x = 0; x < damageDone.Length; x++)
                {
                    int damage = damageDone[x];
                    if (damage != 0)
                    {
                        DamageType damageType = DamageTypes.Instance[x];
                        damageDone[x] = damageTool.GetDamage(damageType, damage, deliveryArguments);
                    }
                }
            }
            else
            {
                ResistanceTool resistanceTool = target.Get<ResistanceTool>();
                float? totalResistance = null;
                for (int x = 0; x < enabledDamageTypes.Length; x++)
                {
                    if (enabledDamageTypes[x])
                    {
                        DamageType damageType = DamageTypes.Instance[x];
                        if (damageType == DamageTypes.Instance.NORMAL)
                        {
                            continue;
                        }
                        float resistance = resistanceTool.GetResistancePercentage(damageType);
                        if (totalResistance == null)
                        {
                            totalResistance = resistance;
                        }
                        else if (resistance > totalResistance.Value)
                        {
                            totalResistance = resistance;
                        }
                    }
                    damageDone[x] = 0;
                }
                if (totalResistance == null)
                {
                    totalResistance = 1f;
                }
                damageDone[(int)combineInto] = (int)(totalResistance.Value * totalDamage);
            }
        }

        public override void Deliver(I_DeliveryTool dOwner, I_DeliveryTool dTarget, DeliveryArgumentPacks deliveryArguments)
        {
            ToolManager target = (dTarget as DeliveryTool).toolManager;
            int x = 0;
            DamageTool damageTool = target.Get<DamageTool>();
            FacultyTool facultyTool = target.Get<FacultyTool>();
            List<Tuple<DamageType, int>> damageList = new();
            foreach (int damage in damageDone)
            {
                if (damage != 0)
                {
                    DamageType damageType = DamageTypes.Instance[x];
                    if ((!facultyTool.Can(Faculties.Instance.recieveDamage) && damage > 0)
                        || (!facultyTool.Can(Faculties.Instance.recieveHealing) && damage < 0))
                    {
                        continue;
                    }
                    damageList.Add(new Tuple<DamageType, int>(damageType, damage));
                }
                x++;
            }
            damageTool.TakeDamage(damageList);
        }

        public override A_DeliveryResult Clone()
        {
            return new DamageResult();
        }
    }
}
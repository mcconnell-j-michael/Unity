using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class EquipmentTool : A_ConfigurableTool<EquipmentTool, EquipmentToolConfiguration>
    {
        private List<DamageType> baseWeaponDamageTypes;

        private List<WeaponDamageTypeChange> weaponDamageTypeOverwrites;
        private List<WeaponDamageTypeAddition> weaponDamageTypeAdditions;

        private List<DamageType> currentDamageTypes;

        public override void Initialize()
        {
            base.Initialize();
            baseWeaponDamageTypes = new List<DamageType>();
            baseWeaponDamageTypes.AddRange(Config.BaseDamageTypes);
            weaponDamageTypeOverwrites = new List<WeaponDamageTypeChange>();
            weaponDamageTypeAdditions = new List<WeaponDamageTypeAddition>();
            currentDamageTypes = new List<DamageType>();
            currentDamageTypes.AddRange(baseWeaponDamageTypes);
        }

        public List<DamageType> GetWeaponDamageTypes()
        {
            List<DamageType> damageTypes = new List<DamageType>();
            damageTypes.AddRange(currentDamageTypes);
            return damageTypes;
        }

        public void AddWeaponDamageTypeOverride(List<DamageType> damageTypes, string id, int priority)
        {
            weaponDamageTypeOverwrites.Add(new WeaponDamageTypeChange()
            {
                priority = priority,
                source = id,
                damageTypes = damageTypes,
            });
            Recalculate();
        }

        public void RemoveWeaponDamageTypeOverride(string id)
        {
            for (int x = 0; x < weaponDamageTypeOverwrites.Count; x++)
            {
                if (weaponDamageTypeOverwrites[x].source == id)
                {
                    weaponDamageTypeOverwrites.RemoveAt(x);
                    x--;
                }
            }
            Recalculate();
        }

        public void AddWeaponDamageTypeAddition(List<DamageType> damageTypes, string id)
        {
            weaponDamageTypeAdditions.Add(new WeaponDamageTypeAddition()
            {
                damageTypes = damageTypes,
                source = id,
            });
            Recalculate();
        }

        public void RemoveWeaponDamageTypeAddition(string id)
        {
            for (int x = 0; x < weaponDamageTypeAdditions.Count; x++)
            {
                if (weaponDamageTypeAdditions[x].source == id)
                {
                    weaponDamageTypeAdditions.RemoveAt(x);
                    x--;
                }
            }
            Recalculate();
        }

        private void Recalculate()
        {
            currentDamageTypes.Clear();
            List<DamageType> baseDamageTypes = this.baseWeaponDamageTypes;
            WeaponDamageTypeChange? current = null;
            foreach (WeaponDamageTypeChange change in weaponDamageTypeOverwrites)
            {
                if (current == null || change.priority < current.Value.priority)
                {
                    current = change;
                    continue;
                }
                if (change.priority == current.Value.priority)
                {
                    if (change.source.CompareTo(current.Value.source) < 0)
                    {
                        current = change;
                    }
                }
            }
            if (current != null)
            {
                baseDamageTypes = current.Value.damageTypes;
            }
            currentDamageTypes.AddRange(baseDamageTypes);
            foreach (WeaponDamageTypeAddition addition in weaponDamageTypeAdditions)
            {
                foreach (DamageType dt in addition.damageTypes)
                {
                    if (!currentDamageTypes.Contains(dt))
                    {
                        currentDamageTypes.Add(dt);
                    }
                }
            }
        }
    }
}
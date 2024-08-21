using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    [Serializable]
    public class DamagePackBuilder : I_EffectBuilder
    {
        [HideLabel, EnumToggleButtons, OdinSerialize]
        private DamageTypeOption option;
        [OdinSerialize]
        private bool useWeapon;
        [OdinSerialize, HideLabel, EnumSODropdown]
        [ShowIf("@" + nameof(option) + " == " + nameof(DamageTypeOption) + "." + nameof(DamageTypeOption.Singular) +
            " && !" + nameof(useWeapon))]
        private DamageType damageType = default;
        [OdinSerialize, EnumSODropdown, HideLabel, Title("Enabled Damage Types")]
        [ShowIf("@" + nameof(option) + " == " + nameof(DamageTypeOption) + "." + nameof(DamageTypeOption.Collection) +
            " && !" + nameof(useWeapon))]
        private List<DamageType> damageTypes = default;
        [OdinSerialize, HideLabel]
        private I_DeliveryValue deliveryValue;

        public DamagePackBuilder() { }

        public DamagePackBuilder(DamageType damageType, I_DeliveryValue value)
        {
            this.damageType = damageType;
            this.deliveryValue = value;
            option = DamageTypeOption.Singular;
            useWeapon = false;
        }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            EffectsArgumentPack effectArgs = deliveryArguments.GetPack<EffectsArgumentPack>();

            if (option == DamageTypeOption.Singular)
            {
                DamageType newDamageType;
                if (useWeapon)
                {
                    EquipmentTool ownerEt = (owner as DeliveryTool).toolManager.Get<EquipmentTool>();
                    List<DamageType> damageTypes = ownerEt.GetWeaponDamageTypes();
                    if (damageTypes == null || damageTypes.Count <= 0)
                    {
                        Logger.ErrorLog("Weapon damage type could not be resolved, defaulting to normal damage type");
                        newDamageType = DamageTypes.Instance.NORMAL;
                    }
                    else
                    {
                        newDamageType = damageTypes[0];
                    }
                }
                else
                {
                    newDamageType = damageType;
                }
                float damageScale = 1f;
                foreach (ReservedEffectFloatArgument arg in newDamageType.GetScaleEnumerator())
                {
                    damageScale *= effectArgs.GetFloatScale(arg);
                }
                float damageFlat = 0f;
                foreach (ReservedEffectFloatArgument arg in newDamageType.GetFlatEnumerator())
                {
                    damageFlat += effectArgs.GetFloatFlat(arg);
                }
                float total = (damageScale * deliveryValue.Build(owner, target, deliveryArguments)) + damageFlat;
                return new DamagePack(newDamageType, total);
            }
            else
            {
                List<DamageType> newDamageTypes = new();
                if (useWeapon)
                {
                    EquipmentTool ownerEt = (owner as DeliveryTool).toolManager.Get<EquipmentTool>();
                    List<DamageType> damageTypes = ownerEt.GetWeaponDamageTypes();
                    if (damageTypes == null || damageTypes.Count <= 0)
                    {
                        Logger.ErrorLog("Weapon damage type could not be resolved, defaulting to normal damage type");
                        newDamageTypes.Add(DamageTypes.Instance.NORMAL);
                    }
                    newDamageTypes.AddRange(damageTypes);
                }
                else
                {
                    newDamageTypes.AddRange(damageTypes);
                }
                DamageType initial = newDamageTypes[0];
                List<ReservedEffectFloatArgument> scales = new();
                List<ReservedEffectFloatArgument> flats = new();
                scales.AddRange(initial.GetScaleEnumerator());
                flats.AddRange(initial.GetFlatEnumerator());
                foreach (DamageType dt in newDamageTypes)
                {
                    for (int x = 0; x < scales.Count; x++)
                    {
                        if (!dt.HasScale(scales[x]))
                        {
                            scales.RemoveAt(x);
                            x--;
                        }
                    }
                    for (int x = 0; x < flats.Count; x++)
                    {
                        if (!dt.HasFlat(scales[x]))
                        {
                            flats.RemoveAt(x);
                            x--;
                        }
                    }
                }
                float damageScale = 1f;
                foreach (ReservedEffectFloatArgument arg in scales)
                {
                    damageScale *= effectArgs.GetFloatScale(arg);
                }
                float damageFlat = 0f;
                foreach (ReservedEffectFloatArgument arg in flats)
                {
                    damageFlat += effectArgs.GetFloatFlat(arg);
                }
                float total = (damageScale * deliveryValue.Build(owner, target, deliveryArguments)) + damageFlat;
                return new CollectiveDamagePack(newDamageTypes, total);
            }
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Deal [" + deliveryValue.Visualize() + "] OF ";
            if (useWeapon)
            {
                vis += " weapon's damage type";
            }
            else
            {
                vis += damageType.ToString();
            }
            return vis;
        }

        public DamagePackBuilder(SerializationInfo info, StreamingContext context)
        {
            option = (DamageTypeOption)info.GetValue(nameof(option), typeof(DamageTypeOption));
            useWeapon = info.GetBoolean(nameof(useWeapon));
            damageType = DamageTypes.Instance[info.GetInt32(nameof(damageType))];
            damageTypes = StaticUtilities.LoadList(info, nameof(damageTypes), (string name) =>
            {
                return DamageTypes.Instance[info.GetInt32(name)];
            });
            deliveryValue = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(deliveryValue));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(option), option);
            info.AddValue(nameof(useWeapon), useWeapon);
            info.AddValue(nameof(damageType), (int)damageType);
            StaticUtilities.SaveList(info, nameof(damageTypes), damageTypes, (string name, DamageType damageType) =>
            {
                info.AddValue(name, (int)damageType);
            });
            StaticUtilities.SaveInterfaceValue(info, nameof(deliveryValue), deliveryValue);
        }
    }

    [Serializable]
    public enum DamageTypeOption
    {
        Singular, Collection
    }
}
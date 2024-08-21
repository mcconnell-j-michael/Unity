using Ashen.AbilitySystem;
using Ashen.NodeTreeSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.SkillTree
{
    public class Skill : A_NodeElement
    {
        [PropertyRange(1, 10), OnValueChanged(nameof(UpdateChildren))]
        public int skillLevels = 1;

        [EnumToggleButtons, SerializeField, HideLabel]
        public SkillType skillType;

        [Title("Base"), ShowIf(nameof(skillType), Value = SkillType.Ability), Hide, OdinSerialize, NonSerialized]
        public AbilitySkillNode abilitySkillNode;

        [Title("Base"), ShowIf(nameof(skillType), Value = SkillType.Passive), Hide, OdinSerialize, NonSerialized]
        public PassiveSkillNode passiveSkillNode;

        private void UpdateChildren()
        {
            if (abilitySkillNode != null)
            {
                abilitySkillNode.SkillLevel = skillLevels;
            }
            if (passiveSkillNode != null)
            {
                passiveSkillNode.SkillLevel = skillLevels;
            }
        }

        [Button]
        public PassiveContainer GetPassiveForLevel(int level)
        {
            PassiveContainer container = new PassiveContainer();
            if (level <= 0 || level > skillLevels)
            {
                Logger.ErrorLog("Invalid level for skill node! Skill node: " + name + " Max level of skill: " + skillLevels + " Requested Skill level: " + level);
                return container;
            }
            if (skillType != SkillType.Passive)
            {
                return container;
            }
            if (passiveSkillNode.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
            {
                container.builder = passiveSkillNode.baseAbility.statusEffect;
            }
            else
            {
                container.builder = passiveSkillNode.builder;
            }
            for (int x = 1; x <= level; x++)
            {
                PassiveSkillNodeOverride passiveOverride = passiveSkillNode.GetOverride(x);
                if (passiveOverride.options == SkillNodeOverrideOptions.Scale)
                {
                    ScaleDeliveryPack scale = passiveOverride.scaleDeliveryPack;
                    foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                    {
                        if (argument.IsReserved())
                        {
                            continue;
                        }
                        EffectFloatArgument newArg = argument as EffectFloatArgument;
                        if (scale.scale.TryGetValue(newArg, out float value))
                        {
                            container.ScaleDeliveryPacks[(int)argument] = value;
                        }
                    }
                }
                else if (passiveOverride.options == SkillNodeOverrideOptions.New)
                {
                    ReplacePassiveSkill replace = passiveOverride.replaceSkillAbility;
                    if (replace.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
                    {
                        container = new PassiveContainer()
                        {
                            builder = replace.ability.statusEffect,
                        };
                    }
                    else if (replace.type == ReplaceAbilitySkillTypeInspector.Custom)
                    {
                        container = new PassiveContainer()
                        {
                            builder = replace.builder,
                        };
                    }
                }
            }
            return container;
        }

        public Ability GetAbilityForLevel(ToolManager manager, int level)
        {
            if (level <= 0 || level > skillLevels)
            {
                Logger.ErrorLog("Invalid level for skill node! Skill node: " + name + " Max level of skill: " + skillLevels + " Requested Skill level: " + level);
                return null;
            }
            if (skillType != SkillType.Ability)
            {
                return null;
            }
            AbilityBuilder builder = null;
            if (abilitySkillNode.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
            {
                builder = abilitySkillNode.baseAbility.builder;
            }
            else
            {
                builder = abilitySkillNode.builder;
            }
            Ability ability = builder.Build();
            AbilityAction abilityAction = ability.abilityAction;
            AbilityDeliveryPackProcessor deliveryPackProcessor = abilityAction.Get<AbilityDeliveryPackProcessor>();
            for (int x = 1; x <= level; x++)
            {
                AbilitySkillNodeOverride skillOverride = abilitySkillNode.GetOverride(x);
                if (skillOverride.options == SkillNodeOverrideOptions.Scale)
                {
                    ScaleAbility scale = skillOverride.scaleDeliveryPack;
                    foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                    {
                        if (argument.IsReserved())
                        {
                            continue;
                        }
                        EffectFloatArgument newArg = argument as EffectFloatArgument;
                        if (scale.scale.TryGetValue(newArg, out float value))
                        {
                            deliveryPackProcessor.SetEffectFloat(argument, value);
                        }
                    }
                    if (scale.enableAbilityOverride && scale.abilityOverrideContainer != null)
                    {
                        scale.abilityOverrideContainer.Override(abilityAction);
                    }
                }
                else if (skillOverride.options == SkillNodeOverrideOptions.New)
                {
                    ReplaceAbilitySkill replace = skillOverride.replaceSkillAbility;
                    if (replace.type == ReplaceAbilitySkillTypeInspector.ScriptableObject)
                    {
                        ability = replace.ability.builder.Build();
                        abilityAction = ability.abilityAction;
                        if (replace.enableAbilityOverride && replace.abilityOverrideContainer != null)
                        {
                            replace.abilityOverrideContainer.Override(abilityAction);
                        }
                    }
                    else if (replace.type == ReplaceAbilitySkillTypeInspector.Custom)
                    {
                        ability = replace.builder.Build();
                    }
                }

                SubAbilitySkillNodeOverrideList subAbilityList = skillOverride.subAbilitySkillNodeOverrideList;
                if (subAbilityList != null)
                {
                    SubAbilityProcessor subAbilityProcessor = ability.abilityAction.Get<SubAbilityProcessor>();
                    if (subAbilityList.option == SubAbilitySkillNodeOverrideList.SubAbilityOptions.ReplaceAll)
                    {
                        subAbilityProcessor.ResetAbilityActions();
                        foreach (SubAbilityBuilder subBuilder in subAbilityList.replacementSubAbilities)
                        {
                            subAbilityProcessor.AddAbilityAction(subBuilder.Build(ability));
                        }
                    }
                    else if (subAbilityList.option == SubAbilitySkillNodeOverrideList.SubAbilityOptions.Individual)
                    {
                        List<AbilityAction> subAbilityActions = subAbilityProcessor.GetAbilityActions(manager);
                        for (int subX = 0; subX < subAbilityActions.Count && subX < subAbilityList.subAbilities.Count; subX++)
                        {
                            SubAbilitySkillNodeOverride subOverride = subAbilityList.subAbilities[subX];
                            if (subOverride == null)
                            {
                                continue;
                            }
                            if (subOverride.options == SkillNodeOverrideOptions.Scale)
                            {
                                ScaleSubAbility scaleSub = subOverride.scaleDeliveryPack;
                                AbilityAction subAction = subAbilityActions[subX];
                                AbilityDeliveryPackProcessor subDeliveryPackProcessor = subAction.Get<AbilityDeliveryPackProcessor>();
                                foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
                                {
                                    if (argument.IsReserved())
                                    {
                                        continue;
                                    }
                                    EffectFloatArgument newArg = argument as EffectFloatArgument;
                                    if (scaleSub.scale.TryGetValue(newArg, out float value))
                                    {
                                        subDeliveryPackProcessor.SetEffectFloat(newArg, value);
                                    }
                                }
                                if (scaleSub.enableAbilityOverride && scaleSub.abilityOverrideContainer != null)
                                {
                                    scaleSub.abilityOverrideContainer.Override(subAction);
                                }
                            }
                            else if (subOverride.options == SkillNodeOverrideOptions.New)
                            {
                                AbilityAction newAction = subOverride.replaceSkillAbility.Build(ability);
                                subAbilityActions[subX] = newAction;
                            }

                        }
                    }
                }
            }
            foreach (I_AbilityProcessor processor in abilityAction.GetProcessors())
            {
                if (processor != null)
                {
                    processor.OnLoad(abilityAction.abilityArguments);
                }
            }
            return ability;
        }

        [Button]
        private Ability GetAbilityForLevelTest(int level)
        {
            return GetAbilityForLevel(null, level);
        }

        public enum SkillType
        {
            Ability, Passive
        }
    }
}
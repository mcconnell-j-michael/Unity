using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.SkillTree
{
    [Serializable]
    public abstract class A_SkillNode<T> where T : class, new()
    {
        [HideLabel, EnumToggleButtons, PropertyOrder(-1)]
        public ReplaceAbilitySkillTypeInspector type;

        [OdinSerialize, ReadOnly]
        private int skillLevel;
        public int SkillLevel
        {
            set
            {
                skillLevel = value;
            }
        }

        [ValueToggleButton("@" + nameof(BuildValueToggle) + "()"), Hide]
        public int choice;

        public List<ValueDropdownItem<int>> BuildValueToggle()
        {
            List<ValueDropdownItem<int>> options = new List<ValueDropdownItem<int>>();
            for (int x = 1; x <= skillLevel; x++)
            {
                options.Add(new ValueDropdownItem<int>
                {
                    Value = x,
                    Text = "Level " + x,
                });
            }
            if (options.Count == 0)
            {
                choice = -1;
            }
            return options;
        }

        public T GetOverride(int level)
        {

            return Skills[level - 1];
        }

        [HideInInspector]
        public List<T> skills;

        public List<T> Skills
        {
            get
            {
                if (skills == null)
                {
                    skills = new List<T>();
                }
                if (skills.Count < skillLevel)
                {
                    while (skills.Count < skillLevel)
                    {
                        skills.Add(new T());
                    }
                }
                return skills;
            }
        }

        private string BuildTitle()
        {
            return "Level " + choice;
        }

        [OdinSerialize, Title("@" + nameof(BuildTitle) + "()"), Hide]
        public T FocusedSkill
        {
            get
            {
                if (choice > 0 && (choice - 1) < Skills.Count)
                {
                    return Skills[choice - 1];
                }
                return null;
            }
            set
            {
                if (choice > 0 && (choice - 1) < Skills.Count)
                {
                    Skills[choice - 1] = value;
                }
            }
        }
    }
}
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public abstract class A_AbilityBuilder<T> where T : class, I_BaseAbilityBuilder
    {
        public bool listView;

        [OdinSerialize, NonSerialized]
        [OnValueChanged(nameof(OnProcessorBuildersChanged)), ShowIf(nameof(listView))]
        public List<T> processorBuilders;

        public List<T> ProcessorBuilders
        {
            get
            {
                processorBuilders ??= new List<T>();
                return processorBuilders;
            }
        }

        public void FillAbilityAction(Ability ability, AbilityAction abilityAction)
        {
            if (processorBuilders != null)
            {
                foreach (T builder in processorBuilders)
                {
                    I_AbilityProcessor processor = builder.Build(ability);
                    if (processor != null)
                    {
                        abilityAction.AddProcessor(processor);
                    }
                }
            }
        }

        [ValueToggleButton("@" + nameof(BuildValueToggle) + "()"), HideIf(nameof(listView)), Hide]
        public int choice;
        public List<ValueDropdownItem<int>> BuildValueToggle()
        {
            List<ValueDropdownItem<int>> options = new();
            AbilityBuilderTabs tabs = AbilityBuilderTabs.Instance;
            foreach (I_BaseAbilityBuilder builder in tabs.builderOrder)
            {
                if (builder is not T)
                {
                    continue;
                }
                int index = -1;
                Type type = builder.GetType();
                for (int x = 0; x < ProcessorBuilders.Count; x++)
                {
                    if (ProcessorBuilders[x] != null && ProcessorBuilders[x].GetType() == type)
                    {
                        index = x;
                    }
                }
                if (index == -1)
                {
                    ProcessorBuilders.Add(builder.CloneBuilder() as T);
                    index = ProcessorBuilders.Count - 1;
                }
                options.Add(new ValueDropdownItem<int>
                {
                    Value = index,
                    Text = builder.GetTabName(),
                });
            }
            if (options.Count == 0)
            {
                choice = -1;
            }
            return options;
        }

        [ShowInInspector, Title("@" + nameof(GetTabName) + "()"), HideWithoutAutoPopulate, HideIf(nameof(listView))]
        public T FocusedSkill
        {
            get
            {
                if (choice > -1 && (choice) < ProcessorBuilders.Count)
                {
                    return ProcessorBuilders[choice];
                }
                return null;
            }
            set
            {
                if (choice > -1 && (choice) < ProcessorBuilders.Count)
                {
                    ProcessorBuilders[choice] = value;
                }
            }
        }

        private string GetTabName()
        {
            if (FocusedSkill != null)
            {
                return FocusedSkill.GetTabName();
            }
            return "Please choose a Tab";
        }

        public void OnProcessorBuildersChanged()
        {
            if (processorBuilders == null)
            {
                return;
            }
            Dictionary<Type, T> typeMap = new();
            for (int x = 0; x < processorBuilders.Count; x++)
            {
                Type type = processorBuilders[x].GetType();
                if (typeMap.TryGetValue(type, out _))
                {
                    processorBuilders.RemoveAt(x);
                    x--;
                }
                else
                {
                    typeMap[type] = processorBuilders[x];
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace Ashen.AbilitySystem
{
    public class AbilityAction
    {
        public Ability sourceAbility;
        public DeliveryArgumentPacks abilityArguments;

        private Dictionary<Type, I_AbilityProcessor> processors;

        public AbilityAction()
        {
            processors = new Dictionary<Type, I_AbilityProcessor>();
        }

        public void AddProcessor(I_AbilityProcessor processor)
        {
            Type type = processor.GetType();
            if (!processors.ContainsKey(type))
            {
                processors.Add(type, processor);
            }
        }

        public T Get<T>() where T : class, I_AbilityProcessor
        {
            if (processors.TryGetValue(typeof(T), out I_AbilityProcessor value))
            {
                return value as T;
            }
            return null;
        }

        public IEnumerable<I_AbilityProcessor> GetProcessors()
        {
            foreach (I_AbilityProcessor processor in processors.Values)
            {
                if (processor != null)
                {
                    yield return processor;
                }
            }
        }
    }
}
using Ashen.AbilitySystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Ashen.EquationSystem
{
    public class EquationArgumentPack : A_DeliveryArgumentPack<EquationArgumentPack>
    {
        private Dictionary<string, I_EquationArgument> equationArguments;
        private AttributeLimiter passthroughAttributeLimiter;
        private List<AbilityTag> abilityTags;

        public EquationArgumentPack()
        {
            equationArguments = new Dictionary<string, I_EquationArgument>();
        }

        public override I_DeliveryArgumentPack Initialize()
        {
            return new EquationArgumentPack();
        }

        public I_EquationArgument GetArgument(string key)
        {
            if (equationArguments.TryGetValue(key, out I_EquationArgument value))
            {
                return value;
            }
            return null;
        }

        public void AddArgument(string key, I_EquationArgument argument)
        {
            if (equationArguments.ContainsKey(key))
            {
                equationArguments[key] = argument;
            }
            else
            {
                equationArguments.Add(key, argument);
            }
        }

        public AttributeLimiter GetPassthroughAttributeLimiter()
        {
            return passthroughAttributeLimiter;
        }

        public void SetPassthroughAttributeLimiter(AttributeLimiter limiter)
        {
            passthroughAttributeLimiter = limiter;
        }

        public List<AbilityTag> GetAbilityTags()
        {
            if (abilityTags == null)
            {
                abilityTags = new List<AbilityTag>();
            }
            return abilityTags;
        }

        public void SetAbilityTags(List<AbilityTag> tags)
        {
            abilityTags = new List<AbilityTag>();
            abilityTags.AddRange(tags);
        }

        public override void Clear()
        {
            equationArguments.Clear();
        }

        public override void CopyInto(I_DeliveryArgumentPack pack)
        {
            EquationArgumentPack eap = pack as EquationArgumentPack;
            eap.equationArguments.AddRange(equationArguments);
        }
    }
}
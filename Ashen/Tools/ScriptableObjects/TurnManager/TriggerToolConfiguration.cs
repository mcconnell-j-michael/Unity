using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class TriggerToolConfiguration : A_Configuration<TriggerTool, TriggerToolConfiguration>
    {
        [OdinSerialize]
        private Dictionary<ExtendedEffectTrigger, List<I_TriggerEffect>> onTriggerEffects;
        [OdinSerialize, HideIf(nameof(IsDefault))]
        private Dictionary<ExtendedEffectTrigger, List<I_TriggerEffect>> addOnTriggerEffects;

        public Dictionary<ExtendedEffectTrigger, List<I_TriggerEffect>> OnTriggerEffects
        {
            get
            {
                if (onTriggerEffects == null)
                {
                    if (IsDefault())
                    {
                        return null;
                    }
                    return GetDefault().OnTriggerEffects;
                }
                Dictionary<ExtendedEffectTrigger, List<I_TriggerEffect>> retOnTriggerEffects = new();
                foreach (ExtendedEffectTrigger trigger in ExtendedEffectTriggers.Instance)
                {
                    List<I_TriggerEffect> effectsForTrigger = new();
                    if (onTriggerEffects.TryGetValue(trigger, out List<I_TriggerEffect> localEffects))
                    {
                        if (localEffects != null)
                        {
                            effectsForTrigger.AddRange(localEffects);
                        }
                    }
                    else if (!IsDefault() && GetDefault().onTriggerEffects != null && GetDefault().onTriggerEffects.TryGetValue(trigger, out List<I_TriggerEffect> effects))
                    {
                        if (effects != null)
                        {
                            effectsForTrigger.AddRange(effects);
                        }
                    }

                    if (!IsDefault() && addOnTriggerEffects != null && addOnTriggerEffects.TryGetValue(trigger, out List<I_TriggerEffect> additionalEffects))
                    {
                        if (additionalEffects != null)
                        {
                            effectsForTrigger.AddRange(additionalEffects);
                        }
                    }

                    if (effectsForTrigger.Count != 0)
                    {
                        retOnTriggerEffects.Add(trigger, effectsForTrigger);
                    }
                }
                return retOnTriggerEffects;
            }
        }
    }
}
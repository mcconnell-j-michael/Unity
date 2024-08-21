using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class TriggerTool : A_ConfigurableTool<TriggerTool, TriggerToolConfiguration>, I_TriggerListener
    {
        private List<I_TriggerListener>[] triggerListeners;
        private List<I_TriggerEffect>[] triggerToTriggerEffects;
        private bool[] triggerLocks;

        public override void Initialize()
        {
            base.Initialize();
            triggerListeners = new List<I_TriggerListener>[ExtendedEffectTriggers.Count];
            for (int x = 0; x < triggerListeners.Length; x++)
            {
                triggerListeners[x] = new List<I_TriggerListener>();
            }
            Dictionary<ExtendedEffectTrigger, List<I_TriggerEffect>> triggerToEffects = Config.OnTriggerEffects;
            triggerToTriggerEffects = new List<I_TriggerEffect>[ExtendedEffectTriggers.Count];
            triggerLocks = new bool[ExtendedEffectTriggers.Count];
            foreach (ExtendedEffectTrigger trigger in ExtendedEffectTriggers.Instance)
            {
                triggerToTriggerEffects[(int)trigger] = new List<I_TriggerEffect>();
                if (triggerToEffects == null)
                {
                    continue;
                }
                if (triggerToEffects.TryGetValue(trigger, out List<I_TriggerEffect> triggerEffects))
                {
                    if (triggerEffects == null)
                    {
                        continue;
                    }
                    triggerToTriggerEffects[(int)trigger].AddRange(triggerEffects);
                    RegisterTriggerListener(trigger, this);
                }
            }
        }

        public void Trigger(ExtendedEffectTrigger trigger)
        {
            if (triggerLocks[(int)trigger])
            {
                Logger.ErrorLog("Attempted to trigger " + trigger.name + " while it was already being triggered");
                return;
            }
            triggerLocks[(int)trigger] = true;
            foreach (I_TriggerListener listener in triggerListeners[(int)trigger])
            {
                listener.OnTrigger(trigger);
            }
            triggerLocks[(int)trigger] = false;
        }

        public void RegisterTriggerListener(ExtendedEffectTrigger trigger, I_TriggerListener listener)
        {
            triggerListeners[(int)trigger].Add(listener);
        }

        public void UnregisterTriggerListener(ExtendedEffectTrigger trigger, I_TriggerListener listener)
        {
            triggerListeners[(int)trigger].Remove(listener);
        }

        public void OnTrigger(ExtendedEffectTrigger trigger)
        {
            foreach (I_TriggerEffect triggerEffect in triggerToTriggerEffects[(int)trigger])
            {
                triggerEffect.OnTriggerEffect(toolManager, trigger);
            }
        }
    }
}
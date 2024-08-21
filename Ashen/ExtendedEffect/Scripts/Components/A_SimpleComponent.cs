using System;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public abstract class A_SimpleComponent : I_ExtendedEffectComponent
    {
        public virtual void Apply(ExtendedEffect dse, ExtendedEffectContainer container) { }

        public virtual void End(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            Remove(dse, container);
        }

        public static ExtendedEffectTrigger[] statusTriggers = new ExtendedEffectTrigger[] { };
        public virtual ExtendedEffectTrigger[] GetStatusTriggers()
        {
            return statusTriggers;
        }

        public virtual void Remove(ExtendedEffect dse, ExtendedEffectContainer container) { }

        public virtual void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container) { }

        public virtual List<I_ExtendedEffectComponent> BreakDown()
        {
            return null;
        }
    }
}
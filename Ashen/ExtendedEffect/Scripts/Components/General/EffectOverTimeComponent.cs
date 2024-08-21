using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * This BaseStatusEffect will deliver an individual Effect every time
     * the status effect ticks
     **/
    [Serializable]
    public class EffectOverTimeComponent : A_SimpleComponent
    {
        private I_Effect effect;

        public EffectOverTimeComponent() { }
        public EffectOverTimeComponent(I_Effect effect)
        {
            this.effect = effect;
        }

        public override void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container)
        {
            if (statusTrigger == ExtendedEffectTriggers.Instance.Tick)
            {
                Tick(dse);
            }
        }

        private void Tick(ExtendedEffect dse)
        {
            dse.deliveryContainer.AddPrimaryEffect(effect);
        }

        private new static ExtendedEffectTrigger[] statusTriggers = new ExtendedEffectTrigger[] { ExtendedEffectTriggers.Instance.Tick };
        public override ExtendedEffectTrigger[] GetStatusTriggers()
        {
            if (statusTriggers == null)
            {
                statusTriggers = new ExtendedEffectTrigger[0];
            }
            return statusTriggers;
        }

        public EffectOverTimeComponent(SerializationInfo info, StreamingContext context)
        {
            effect = StaticUtilities.LoadInterfaceValue<I_Effect>(info, nameof(effect));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(effect), effect);
        }
    }
}
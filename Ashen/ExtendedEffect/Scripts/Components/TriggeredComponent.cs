using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    [Serializable]
    public class TriggeredComponent : A_SimpleComponent, ISerializable
    {
        private I_Effect effect;
        private ExtendedEffectTrigger[] triggers;
        private int limit;

        public TriggeredComponent() { }

        public TriggeredComponent(I_Effect effect, ExtendedEffectTrigger[] triggers, int limit)
        {
            this.effect = effect;
            this.triggers = triggers;
            this.limit = limit;
        }

        public override void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container)
        {
            for (int x = 0; x < triggers.Length; x++)
            {
                if (statusTrigger == triggers[x])
                {
                    dse.deliveryContainer.AddPrimaryEffect(effect);
                    break;
                }
            }
            if (limit > 0)
            {
                limit--;
                if (limit == 0)
                {
                    dse.RequestDisable();
                }
            }
        }

        public override ExtendedEffectTrigger[] GetStatusTriggers()
        {
            return triggers;
        }

        public TriggeredComponent(SerializationInfo info, StreamingContext context)
        {
            effect = StaticUtilities.LoadInterfaceValue<I_Effect>(info, nameof(effect));
            triggers = StaticUtilities.LoadArray(info, nameof(triggers), (string name) =>
            {
                return ExtendedEffectTriggers.Instance[info.GetInt32(name)];
            });
            limit = info.GetInt32(nameof(limit));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(effect), effect);
            StaticUtilities.SaveArray(info, nameof(triggers), triggers, (string name, ExtendedEffectTrigger trigger) =>
            {
                info.AddValue(name, (int)trigger);
            });
            info.AddValue(nameof(limit), limit);
        }
    }
}
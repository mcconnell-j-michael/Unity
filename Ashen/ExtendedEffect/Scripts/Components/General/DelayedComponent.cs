using Ashen.ObjectPoolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    [Serializable]
    public class DelayedComponent : A_SimpleComponent
    {
        public I_Effect effect;

        public DelayedComponent() { }
        public DelayedComponent(I_Effect effect)
        {
            this.effect = effect;
        }

        public override void End(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryArgumentPacks packs = AGenericPool<DeliveryArgumentPacks>.Get();
            DeliveryContainer deliveryContainer = AGenericPool<DeliveryContainer>.Get();
            deliveryContainer.AddPrimaryEffect(effect);
            DeliveryUtility.Deliver(deliveryContainer, dse.owner, dse.target, packs);
            AGenericPool<DeliveryArgumentPacks>.Release(packs);
            AGenericPool<DeliveryContainer>.Release(deliveryContainer);
        }

        public DelayedComponent(SerializationInfo info, StreamingContext context)
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
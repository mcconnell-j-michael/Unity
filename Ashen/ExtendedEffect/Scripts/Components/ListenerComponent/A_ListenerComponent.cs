using Ashen.DeliverySystem;
using Ashen.ObjectPoolSystem;
using Sirenix.Serialization;

namespace Ashen.ExtendedEffectSystem
{
    public abstract class A_ListenerComponent<T> : A_SimpleComponent
    {
        [OdinSerialize]
        private T listenOn;

        [OdinSerialize, AutoPopulate(instance = typeof(ExtendedEffectBuilder)), HideWithoutAutoPopulate]
        private I_ExtendedEffectBuilder statusEffect;

        public A_ListenerComponent(T listenOn, I_ExtendedEffectBuilder statusEffect)
        {
            this.listenOn = listenOn;
            this.statusEffect = statusEffect;
        }

        public A_ListenerComponent() { }

        private ExtendedEffect currentEffect;
        private I_DeliveryTool owner;
        private I_DeliveryTool target;
        private DeliveryArgumentPacks baseArguments;

        protected void OnListenerTrigger()
        {
            Reapply(currentEffect);
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            owner = dse.owner;
            target = dse.target;
            baseArguments = dse.arguments;
            Register(dse.target, listenOn);
            Reapply(dse);
        }

        private void Reapply(ExtendedEffect dse)
        {
            currentEffect?.Disable(false);
            DeliveryArgumentPacks newArguments = AGenericPool<DeliveryArgumentPacks>.Get();
            baseArguments.CopyInto(newArguments);
            ExtendedEffectArgumentsPack arguments = newArguments.GetPack<ExtendedEffectArgumentsPack>();
            arguments.SetTemp(true);
            currentEffect = statusEffect.Build(owner, target, newArguments);
            currentEffect?.Enable();
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            currentEffect?.Disable(false);
            currentEffect = null;
            Unregister(dse.target, listenOn);
        }

        protected abstract void Register(I_DeliveryTool dt, T toRegister);
        protected abstract void Unregister(I_DeliveryTool dt, T toUnRegister);
    }
}

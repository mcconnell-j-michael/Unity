using Ashen.ObjectPoolSystem;

namespace Ashen.DeliverySystem
{
    public class ApplyEffect : I_TagOperation
    {
        public I_EffectBuilder effect;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryContainer container = AGenericPool<DeliveryContainer>.Get();
            container.AddPrimaryEffect(this.effect.Build(owner, target, deliveryArguments));
            DeliveryArgumentPacks packs = AGenericPool<DeliveryArgumentPacks>.Get();
            DeliveryUtility.Deliver(container, owner, target, packs);
            AGenericPool<DeliveryArgumentPacks>.Release(packs);
            AGenericPool<DeliveryContainer>.Release(container);
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            return visualization + "Apply " + effect.ToString();
        }
    }
}
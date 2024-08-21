namespace Ashen.DeliverySystem
{
    public class DeliveryResultsArgumentPack : A_DeliveryArgumentPack<DeliveryResultsArgumentPack>
    {
        public float criticalChance;
        public float hitChance;
        public bool critical;
        public bool miss;

        private DeliveryResultPack pack;

        public DeliveryResultsArgumentPack()
        {
            pack = new DeliveryResultPack();
        }

        public override I_DeliveryArgumentPack Initialize()
        {
            return new DeliveryResultsArgumentPack();
        }

        public DeliveryResultPack GetDeliveryResultPack()
        {
            return pack;
        }

        public override void Clear()
        {
            pack.Clear();
            criticalChance = 0f;
            hitChance = 100f;
            critical = false;
            miss = false;
        }

        public override void CopyInto(I_DeliveryArgumentPack pack)
        {
            DeliveryResultsArgumentPack drp = pack as DeliveryResultsArgumentPack;
            drp.criticalChance = criticalChance;
            drp.hitChance = hitChance;
            drp.critical = critical;
            drp.miss = miss;
        }
    }
}
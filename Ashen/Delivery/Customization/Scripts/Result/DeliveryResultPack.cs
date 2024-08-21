using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * The DeliveryResultPack contains a primary and a secondary delivery results. The primary is used 
     * while delivering primary delivery packs while the secondary is used while delivering secondary
     * delivery results. This class also manages cleaning up and reusing old DeliverResultPacks.
     **/
    public class DeliveryResultPack
    {
        /*
        private static int initialPacks = 100;
        private static List<DeliveryResultPack> availableDeliveryResultPacks;

        public static DeliveryResultPack GetPack()
        {
            if (availableDeliveryResultPacks == null)
            {
                availableDeliveryResultPacks = new List<DeliveryResultPack>(initialPacks);
                
                for (int x = 0; x < initialPacks; x++)
                {
                    availableDeliveryResultPacks.Add(new DeliveryResultPack());
                }
            }
            DeliveryResultPack drp;
            if (availableDeliveryResultPacks.Count == 0)
            {
                drp = new DeliveryResultPack();
                return drp;
            }
            drp = availableDeliveryResultPacks[availableDeliveryResultPacks.Count - 1];
            availableDeliveryResultPacks.RemoveAt(availableDeliveryResultPacks.Count - 1);
            return drp;
        }
        */
        public A_DeliveryResult[] DeliveryResults { get; private set; }
        public bool empty = true;

        public DeliveryResultPack()
        {
            DeliveryResults = new A_DeliveryResult[DeliveryResultTypes.Count];
            for (int x = 0; x < DeliveryResults.Length; x++)
            {
                DeliveryResults[x] = DeliveryResultTypes.Instance[x].deliveryResult.Clone();
            }
        }

        public T GetResult<T>(DeliveryResultType deliveryResultType) where T:A_DeliveryResult
        {
            return DeliveryResults[(int)deliveryResultType] as T;
        }

        public void Calculate(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            foreach (A_DeliveryResult deliveryResult in DeliveryResults)
            {
                if (deliveryResult != null)
                {
                    deliveryResult.Calculate(owner, target, deliveryArguments);
                }
            }
        }

        public void Deliver(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            foreach (A_DeliveryResult deliveryResult in DeliveryResults)
            {
                if (deliveryResult != null)
                {
                    deliveryResult.Deliver(owner, target, deliveryArguments);
                }
            }
            //availableDeliveryResultPacks.Add(this);
        }

        public void Clear()
        {
            foreach (A_DeliveryResult deliveryResult in DeliveryResults)
            {
                deliveryResult.Clear();
            }
            //availableDeliveryResultPacks.Add(this);
        }
    }
}

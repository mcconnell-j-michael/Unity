using Ashen.ToolSystem;

namespace Ashen.DeliverySystem
{
    public class ResourceChangeResult : A_DeliveryResult
    {
        private bool[] enabledResourceChanges;
        private int[] resourceChanges;

        public ResourceChangeResult()
        {
            enabledResourceChanges = new bool[ResourceValues.Count];
            resourceChanges = new int[ResourceValues.Count];
        }

        public void AddResourceChange(ResourceValue resourceValue, int total)
        {
            enabledResourceChanges[(int)resourceValue] = true;
            resourceChanges[(int)resourceValue] += total;
        }

        public override void Calculate(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        { }

        public override void Clear()
        {
            for (int x = 0; x < ResourceValues.Count; x++)
            {
                enabledResourceChanges[x] = false;
                resourceChanges[x] = 0;
            }
        }

        public override A_DeliveryResult Clone()
        {
            return new ResourceChangeResult();
        }

        public override void Deliver(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool tDeliveryTool = target as DeliveryTool;
            ResourceValueTool rvTool = tDeliveryTool.toolManager.Get<ResourceValueTool>();
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                if (!enabledResourceChanges[(int)resourceValue])
                {
                    continue;
                }
                int value = resourceChanges[(int)resourceValue];
                if (value > 0)
                {
                    rvTool.RemoveAmount(resourceValue, value);
                }
                else if (value < 0)
                {
                    rvTool.ApplyAmount(resourceValue, -value);
                }
            }
        }
    }
}
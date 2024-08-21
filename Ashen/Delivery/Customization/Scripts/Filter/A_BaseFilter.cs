using System;

namespace Ashen.DeliverySystem
{
    /**
     * This is the class that defines all BaseFilters. By default they are enabled
     **/
    [Serializable]
    public abstract class A_BaseFilter : I_Filter
    {
        public abstract bool Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentsPack, DeliveryResultPack deliveryResult);

        public virtual bool Enabled()
        {
            return true;
        }
    }
}
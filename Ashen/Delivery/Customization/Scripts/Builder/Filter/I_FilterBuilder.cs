using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_FilterBuilder
    {
        I_Filter Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments);
    }
}
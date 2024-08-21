using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * This interface is used to create a new type of DynamicDamageType
     **/
    public interface I_DynamicDamageType
    {
        List<DamageRatio> GetDamageTypes(ToolManager target);
    }
}

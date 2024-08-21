using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.ToolSystem
{
    [CreateAssetMenu(fileName = nameof(ResourceValue), menuName = "Custom/Enums/" + nameof(ResourceValues) + "/Type")]
    public class ResourceValue : A_Attribute<ResourceValue, ResourceValues, float>, I_EquationAttribute<ResourceValueTool, ResourceValue, float>
    {
        [EnumSODropdown]
        public List<DamageType> listenOn;
        [Hide, Title("Threshold")]
        public ThresholdBuilder threshold;

        public override float Get(I_DeliveryTool deliveryTool)
        {
            ToolManager toolManager = (deliveryTool as DeliveryTool).toolManager;
            if (toolManager)
            {
                ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
                if (resourceValueTool)
                {
                    return resourceValueTool.Get(this, null);
                }
            }
            return 0;
        }

        public float GetAsFloat(float value)
        {
            return value;
        }

        public override string GetAttributeType()
        {
            return nameof(ResourceValue);
        }
    }
}
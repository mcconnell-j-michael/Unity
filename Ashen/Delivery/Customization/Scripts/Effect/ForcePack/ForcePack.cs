using Ashen.EquationSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    public class ForcePack : I_Effect
    {
        [HorizontalGroup(nameof(ForcePack), width: 0.25f), EnumToggleButtons, HideLabel]
        public ForceType forceType;
        [HorizontalGroup(nameof(ForcePack)), HideReferenceObjectPicker, InlineProperty, LabelWidth(150)]
        public Equation forceMagnitude;

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            Vector3? collisionNullable = deliveryArguments.GetCollisionSource();
            if (collisionNullable == null)
            {
                return;
            }
            ToolManager targetTM = (target as DeliveryTool).toolManager;
            //A_BaseController controller = targetTM.Get<A_BaseController>();
            //if (!controller)
            //{
            //   return;
            // }
            Vector3 collision = (Vector3)collisionNullable;
            Vector2 direction = new Vector2(targetTM.gameObject.transform.position.x - collision.x, 1f);
            Vector2 force = direction.normalized * forceMagnitude.Calculate(owner, deliveryArguments);
            // controller.ApplyKnockBack(force);
        }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return this;
        }

        public string visualize(int depth)
        {
            return "NOT YET DONE";
        }
    }

    public enum ForceType
    {
        PUSH, PULL
    }
}
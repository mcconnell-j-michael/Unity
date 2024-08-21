using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

/**
 * The Scriptable object used to create a StatusEffect
 **/
[CreateAssetMenu(fileName = "StatusEffect", menuName = "Custom/CombatInfrastructure/StatusEffect", order = 1)]
public class StatusEffectScriptableObject : SerializedScriptableObject
{
    [OdinSerialize, AutoPopulate(instance = typeof(ExtendedEffectBuilder)), HideWithoutAutoPopulate]
    public I_ExtendedEffectBuilder statusEffect;

    public I_ExtendedEffect Clone(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks)
    {
        return statusEffect.Build(owner, target, deliveryArgumentPacks);
    }

    public override string ToString()
    {
        return name;
    }
}

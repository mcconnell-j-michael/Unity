using UnityEngine;

namespace Ashen.DeliverySystem
{
    [CreateAssetMenu(fileName = nameof(ExtendedEffectTags), menuName = "Custom/Enums/" + nameof(ExtendedEffectTags) + "/Types")]
    public class ExtendedEffectTags : A_EnumList<ExtendedEffectTag, ExtendedEffectTags>
    { }
}